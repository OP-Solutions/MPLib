using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using MPLib.Helper;
using MPLib.Models.Games;
using ProtoBuf;

namespace MPLib.Networking
{
    /// <summary>
    /// Class which manager card deck. This class is intended to be used in various card game logic implementation
    /// This class is responsible for encrypting-shuffling cards, decrypting it only for specific player or decrypting publicly
    /// And also if someone would cheat or does any mistake in card encryption this class will be able to identify it
    /// </summary>
    /// <remarks>
    /// TODO: need to test this class and debug if needed
    /// </remarks>
    /// <typeparam name="TPlayerType"></typeparam>
    /// <typeparam name="TBaseMessageType"></typeparam>
    public class PlayerMessageManager<TBaseMessageType> : IPlayerMessageManager<TBaseMessageType> where TBaseMessageType : IMessage
    {

        private readonly PlayerIdentifyMode _identifyMode;
        private readonly ECDsa _signer;
        private readonly Player[] _connectedPlayers;
        private readonly Player _myPlayer;
        private readonly TypeCodeMapper _messageTypeCodeMapper;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly byte[] _internalBuffer = new byte[1024 * 1024];
        private readonly Dictionary<Player, Dictionary<Type, Channel<Package>>> _messageStorage;


        /// (<see cref="Package.SenderIdentifier"/>/>, (<see cref="Package.DestinationIdentifier"/>/>,
        /// </param>
        /// <param name="players">Contact list to send messages to</param>
        /// <param name="messageTypeTypeCodeMapper">
        /// This is like dictionary which map types to number's, which are added as prefix when serialized
        /// So deserializer party knows which type was sent and correctly deserializes message
        /// </param>
        internal PlayerMessageManager(IReadOnlyList<Player> players, TypeCodeMapper messageTypeTypeCodeMapper, PlayerIdentifyMode identifyMode)
        {
            _messageTypeCodeMapper = messageTypeTypeCodeMapper;
            _connectedPlayers = players.Where(p => !p.IsMyPlayer).ToArray();
            _myPlayer = players.First(p => p.IsMyPlayer);
            _signer = new ECDsaCng(_myPlayer.Key);
            _messageStorage = new Dictionary<Player, Dictionary<Type, Channel<Package>>>(_connectedPlayers.Length);
            foreach (var player in _connectedPlayers)
            {
                _messageStorage[player] = new Dictionary<Type, Channel<Package>>();
            }
            _identifyMode = identifyMode;
        }


        /// <summary>
        /// This method starts continuously reading messages from other player (<see cref="_connectedPlayers"/>),
        /// Cryptographically verifies their signature, timestamp, ensures correct format
        /// And saves them in storage indexed my message type, so user of thi class can read anytime with <see cref="ReadMessageFrom{T}"/>
        /// </summary>
        /// <remarks>
        /// TODO: timestamp check and packet relay to other destination need to be done
        /// </remarks>
        public void StartAsyncReading()
        {
            for (var i = 0; i < _connectedPlayers.Length; i++)
            {
                var player = _connectedPlayers[i];
                var playerIndex = i;
                Task.Run(() => HandleMessageStreamFromPlayer(player, playerIndex), _cancellationTokenSource.Token);
            }
        }
        private async Task HandleMessageStreamFromPlayer(Player player, int playerIndex)
        {
            var networkStream = player.NetworkStream;
            var memStream = new MemoryStream(_internalBuffer);
            while (true)
            {
                if (await HandleMessageFromPlayer(player, playerIndex, networkStream, memStream))
                    break;
            }
        }
        private async Task<bool> HandleMessageFromPlayer(Player player, int playerIndex, Stream networkStream, Stream stream)
        {

            //data length (without signature)
            var dataLen = await networkStream.ReadBytesOpaque16Async(_internalBuffer, 0);
            if (dataLen == 0) return true;
            var messageTypeCode = await networkStream.ReadInt16Async();
            var messageType = _messageTypeCodeMapper.GetType(messageTypeCode);
            Serializer.NonGeneric.TryDeserializeWithLengthPrefix(stream, PrefixStyle.Fixed32BigEndian, _ => typeof(Package), out var packageObj);
            var package = (Package)packageObj;
            Serializer.NonGeneric.TryDeserializeWithLengthPrefix(stream, PrefixStyle.Fixed32BigEndian, _ => messageType, out var message);

            var signature = await CheckSignature(networkStream, player, playerIndex, dataLen);

            CheckSender(package, playerIndex);

            package.SenderSignature = signature;
            package.Message = message;

            var channel = GetChannel(player, messageType);
            await channel.Writer.WriteAsync(package);
            return false;
        }

        private async Task<byte[]> CheckSignature(Stream networkStream, Player player, int playerIndex, int dataLen)
        {
            var signature = await networkStream.ReadBytesOpaque16Async();
            if (signature == null)
                throw new Exception($"Can't read network message signature from player: {player} (index={playerIndex}");

            var signer = new ECDsaCng(player.Key);

            if (!signer.VerifyData(_internalBuffer, 0, dataLen, signature, HashAlgorithmName.SHA256))
                throw new Exception($"Invalid signature of network message from player: {player} (index={playerIndex}");
            return signature;
        }
        private void CheckSender(Package package, int playerIndex)
        {

            var senderIdentifier = package.SenderIdentifier;

            switch (_identifyMode)
            {

                case PlayerIdentifyMode.Index:
                    var deserializedIndex = BitConverter.ToInt16(senderIdentifier);
                    if (deserializedIndex != playerIndex) throw new Exception("Incorrect sender indicated");
                    break;
                case PlayerIdentifyMode.PublicKey:
                    break;
                default:
                    throw new NotSupportedException($"Unknown identify mode: {_identifyMode}");
            }
        }

        public async Task BroadcastMessage(TBaseMessageType message)
        {
            var tasks = (from player in _connectedPlayers
                         select SendMessageTo(player, message)).ToList();

            await Task.WhenAll(tasks);
        }

        public async Task SendMessageTo(Player player, TBaseMessageType message)
        {

            byte[] identifier;

            switch (_identifyMode)
            {
                case PlayerIdentifyMode.Index:
                {
                    var index = Array.IndexOf(_connectedPlayers, player);
                    if(index == -1) throw new Exception("Invalid player");
                    identifier = BitConverter.GetBytes((short)index);
                    break;
                }
                case PlayerIdentifyMode.PublicKey:
                    identifier = player.PublicKeyBytes;
                    break;
                default:
                    throw new NotSupportedException($"Unknown Player identify mode: {_identifyMode}");
            }

            var package = new Package()
            {
                SenderIdentifier = _myPlayer.Key.Export(CngKeyBlobFormat.EccPublicBlob),
                DestinationIdentifier = identifier,
                Message = message
            };

            var buffer = ArrayPool<byte>.Shared.Rent(1024 * 32);
            try
            {
                await using var stream = new MemoryStream(buffer) { Position = 0 };
                await stream.WriteInt16Async(_messageTypeCodeMapper.GetCode(message.GetType()));
                Serializer.NonGeneric.SerializeWithLengthPrefix(stream, package, PrefixStyle.Fixed32BigEndian, 0);
                Serializer.NonGeneric.SerializeWithLengthPrefix(stream, message, PrefixStyle.Fixed32BigEndian, 0);
                var signature = _signer.SignData(buffer, 0, (int)stream.Position, HashAlgorithmName.SHA256);
                var networkStream = player.NetworkStream;
                await networkStream.WriteInt16Async((short)stream.Position);
                await stream.CopyToAsync(networkStream);
                await networkStream.WriteBytesOpaque16Async(signature);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            
        }

        public async Task<T> ReadMessageFrom<T>(Player player) where T : TBaseMessageType
        {
            var channel = GetChannel(player, typeof(T));
            var package = await channel.Reader.ReadAsync();
            return (T)package.Message;
        }


        private Channel<Package> GetChannel(Player player, Type messageType)
        {

            Channel<Package> channel;
            lock (_messageStorage)
            {
                var playerMessageStorage = _messageStorage[player];
                if (!playerMessageStorage.TryGetValue(messageType, out channel))
                {
                    channel = Channel.CreateBounded<Package>(100);
                    playerMessageStorage[messageType] = channel;
                }
            }
            return channel;
        }

    }

    interface IPlayerMessageManager<in TBaseMessageType> : IPlayerMessageSender<TBaseMessageType>, 
        IPlayerMessageReceiver<TBaseMessageType> 
        where TBaseMessageType : IMessage
    {
    }

    interface IPlayerMessageSender<in TBaseMessageType> where TBaseMessageType : IMessage
    {
        Task SendMessageTo(Player player, TBaseMessageType message);
        Task BroadcastMessage(TBaseMessageType message);
    }

    interface IPlayerMessageReceiver<in TBaseMessageType> where TBaseMessageType : IMessage
    {
        Task<T> ReadMessageFrom<T>(Player player) where T : TBaseMessageType;
    }

    public enum PlayerIdentifyMode
    {

        /// <summary>
        /// When player sends message to other player he writes his index in players list in <see cref="Package.SenderIdentifier"/>
        /// and index of receiver in <see cref="Package.DestinationIdentifier"/>
        /// </summary>
        Index,

        /// <summary>
        /// When player sends message to other player he writes his public key in <see cref="Package.SenderIdentifier"/>
        /// and public key of receiver in <see cref="Package.DestinationIdentifier"/>
        /// </summary>
        PublicKey,
    }
}
