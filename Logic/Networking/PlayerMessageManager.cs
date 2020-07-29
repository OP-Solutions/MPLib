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
    internal class PlayerMessageManager<TPlayerType, TBaseMessageType> : IPlayerMessageManager<TPlayerType, TBaseMessageType> where TBaseMessageType : IMessage where TPlayerType : Player
    {

        private readonly ECDsa _signer;
        private readonly IReadOnlyList<TPlayerType> _connectedPlayers;
        private readonly TPlayerType _myPlayer;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly byte[] _internalBuffer = new byte[1024 * 1024];
        private readonly Dictionary<Player, Dictionary<Type, Channel<Package>>> _messageStorage;
        private readonly TypeCodeMapper _messageTypeCodeMapper;

        /// <param name="otherPlayers">
        /// players dictionary. key is player itself, value - corresponding player identifier, which is included in each message
        /// so remote party knows who message are coming from, or who is destination of that message:
        /// (<see cref="Package{TBaseMessageType}.SenderIdentifier"/>/>, (<see cref="Package{TBaseMessageType}.DestinationIdentifier"/>/>,
        /// </param>
        /// <param name="players">Contact list to send messages to</param>
        /// <param name="messageTypeTypeCodeMapper">
        /// This is like dictionary which map types to number's, which are added as prefix when serialized
        /// So deserializer party knows which type was sent and correctly deserializes message
        /// </param>
        public PlayerMessageManager(IReadOnlyList<TPlayerType> players, TypeCodeMapper messageTypeTypeCodeMapper)
        {
            _messageTypeCodeMapper = messageTypeTypeCodeMapper;
            _connectedPlayers = players.Where(p => !p.IsMyPlayer).ToArray();
            _myPlayer = players.First(p => p.IsMyPlayer);
            _signer = new ECDsaCng(_myPlayer.Key);
            _messageStorage = new Dictionary<Player, Dictionary<Type, Channel<Package>>>(_connectedPlayers.Count);
            foreach (var player in _connectedPlayers)
            {
                _messageStorage[player] = new Dictionary<Type, Channel<Package>>();
            }
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
            foreach (var player in _connectedPlayers)
            {
                Task.Run(async () =>
                {
                    var networkStream = player.NetworkStream;
                    var memStream = new MemoryStream(_internalBuffer);
                    while (true)
                    {
                        //data length (without signature)
                        var dataLen = await networkStream.ReadBytesOpaque16Async(_internalBuffer, 0);
                        if(dataLen == 0) break;
                        var messageTypeCode = await networkStream.ReadInt16Async();
                        var messageType = _messageTypeCodeMapper.GetType(messageTypeCode);
                        Serializer.NonGeneric.TryDeserializeWithLengthPrefix(memStream, PrefixStyle.Fixed32BigEndian, 
                            _ => typeof(Package), out var packageObj);
                        var package = (Package)packageObj;
                        Serializer.NonGeneric.TryDeserializeWithLengthPrefix(memStream, PrefixStyle.Fixed32BigEndian, 
                            _ => messageType, out var message);


                        var signature = await networkStream.ReadBytesOpaque16Async();
                        if (signature == null) break;

                        var signer = new ECDsaCng(player.Key);

                        if(!signer.VerifyData(_internalBuffer, 0, dataLen, signature, HashAlgorithmName.SHA256))
                            break;

                        package.SenderSignature = signature;
                        package.Message = message;

                        var channel = GetChannel(player, messageType);
                        await channel.Writer.WriteAsync(package);
                    }
                }, _cancellationTokenSource.Token);
            }
        }
        
        public async Task BroadcastMessage(TBaseMessageType message)
        {
            var tasks = (from TPlayerType player in _connectedPlayers
                         select SendMessageTo(player, message)).ToList();

            await Task.WhenAll(tasks);
        }

        public async Task SendMessageTo(TPlayerType player, TBaseMessageType message)
        {
            var package = new Package()
            {
                SenderIdentifier = _myPlayer.Key.Export(CngKeyBlobFormat.EccPublicBlob),
                DestinationIdentifier = player.PublicKeyBytes,
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

        public async Task<T> ReadMessageFrom<T>(TPlayerType player) where T : TBaseMessageType
        {
            var channel = GetChannel(player, typeof(T));
            var package = await channel.Reader.ReadAsync();
            return (T)package.Message;
        }


        private Channel<Package> GetChannel(TPlayerType player, Type messageType)
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

    public interface IPlayerMessageManager<in TPlayerType, in TBaseMessageType> : IPlayerMessageSender<TPlayerType, TBaseMessageType>, 
        IPlayerMessageReceiver<TPlayerType, TBaseMessageType> 
        where TBaseMessageType : IMessage
        where TPlayerType : Player
    {
    }

    public interface IPlayerMessageSender<in TPlayerType, in TBaseMessageType> where TBaseMessageType : IMessage
    {
        Task SendMessageTo(TPlayerType player, TBaseMessageType message);
        Task BroadcastMessage(TBaseMessageType message);
    }

    public interface IPlayerMessageReceiver<in TPlayerType, in TBaseMessageType> where TBaseMessageType : IMessage
    {
        Task<T> ReadMessageFrom<T>(TPlayerType player) where T : TBaseMessageType;
    }
}
