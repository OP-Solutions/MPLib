using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Schema;
using EtherBetClientLib.Core.Game.Poker.Messaging;
using EtherBetClientLib.Helper;
using EtherBetClientLib.Models;
using EtherBetClientLib.Models.Games;
using ProtoBuf;

namespace EtherBetClientLib.Networking
{
    public class PlayerMessageManager<TPlayerType, TBaseMessageType> : IPlayerMessageManager<TPlayerType, TBaseMessageType> where TBaseMessageType : IMessage where TPlayerType : Player
    {

        private readonly string _myIdentifier;
        private readonly ECDsa _signer;
        private readonly IReadOnlyDictionary<TPlayerType, string> _connectedPlayers;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly byte[] _internalBuffer = new byte[1024 * 1024];
        private readonly Dictionary<Player, Dictionary<Type, Channel<Package>>> _messageStorage;


        // TODO need to implement thi dictionary fill
        private readonly IReadOnlyDictionary<short, Type> _messageCodeToTypeMap = new Dictionary<short, Type>();

        /// <param name="otherPlayers">
        /// players dictionary. key is player itself, value - corresponding player identifier, which is included in each message
        /// so remote party knows who message are coming from, or who is destination of that message:
        /// (<see cref="Package{TBaseMessageType}.SenderIdentifier"/>/>, (<see cref="Package{TBaseMessageType}.DestinationIdentifier"/>/>,
        /// </param>
        /// <param name="players">Contact list to send messages to</param>
        public PlayerMessageManager(IReadOnlyDictionary<TPlayerType, string> players)
        {
            _connectedPlayers = players.Where(p => !p.Key.IsMyPlayer).ToDictionary(p => p.Key, p => p.Value);
            var myPlayerInfo = players.First(p => p.Key.IsMyPlayer);
            var myPlayer = myPlayerInfo.Key;
            _myIdentifier = myPlayerInfo.Value;
            _signer = new ECDsaCng(myPlayer.Key);
            _messageStorage = new Dictionary<Player, Dictionary<Type, Channel<Package>>>(_connectedPlayers.Count);
            foreach (var (player, _) in _connectedPlayers)
            {
                _messageStorage[player] = new Dictionary<Type, Channel<Package>>();
            }
        }

        public void StartAsyncReading()
        {
            foreach (var (player, _) in _connectedPlayers)
            {
                Task.Run(async () =>
                {
                    var controller = new StreamController(player.NetworkStream);
                    var memStream = new MemoryStream(_internalBuffer);
                    while (true)
                    {
                        //data length (without signature)
                        var dataLen = await controller.ReadBytesOpaque16Async(_internalBuffer, 0);
                        if(dataLen == 0) break;
                        var messageTypeCode = await controller.ReadInt16Async();
                        var messageType = _messageCodeToTypeMap[messageTypeCode];
                        var package = (Package)DeserializePrefixedLenInt16(typeof(Package), memStream);
                        var message = DeserializePrefixedLenInt16(messageType, memStream);


                        var signature = await controller.ReadBytesOpaque16Async();
                        if (signature == null) break;

                        if(!_signer.VerifyData(_internalBuffer, 0, dataLen, signature, HashAlgorithmName.SHA256))
                            break;

                        package.SenderSignature = signature;
                        package.Message = message;
                        var playerMessageStorage = _messageStorage[player];
                        if (!playerMessageStorage.TryGetValue(messageType, out var channel))
                        {
                            channel = Channel.CreateUnbounded<Package>();
                            playerMessageStorage[messageType] = channel;
                        }
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
                SenderIdentifier = _myIdentifier,
                DestinationIdentifier = _connectedPlayers[player],
                Message = message
            };

            var buffer = ArrayPool<byte>.Shared.Rent(1024 * 32);
            try
            {
                await using var stream = new MemoryStream(buffer) { Position = 0 };
                Serializer.NonGeneric.SerializeWithLengthPrefix(stream, package, PrefixStyle.Fixed32BigEndian, 0);
                Serializer.NonGeneric.SerializeWithLengthPrefix(stream, message, PrefixStyle.Fixed32BigEndian, 0);
                var signature = _signer.SignData(buffer, 0, (int)stream.Position, HashAlgorithmName.SHA256);
                var controller = new StreamController(stream);
                await controller.WriteBytesOpaque16Async(signature);
                await stream.CopyToAsync(player.NetworkStream);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            
        }

        public Task<T> ReadMessageFrom<T>(TPlayerType player) where T : TBaseMessageType
        {
            throw new NotImplementedException();
        }


        //Note: not preserves initial length value
        private static object DeserializePrefixedLenInt16(Type type, MemoryStream source)
        {
            if (!Serializer.TryReadLengthPrefix(source, PrefixStyle.Base128, out var length))
                throw new Exception("Unexpected data from stream, can't read length prefix");

            source.SetLength(source.Position + length);
            return Serializer.Deserialize(type, source);
        }

    }

    public interface IPlayerMessageManager<in TPlayerType, in TBaseMessageType> : IPlayerMessageSender<TPlayerType, TBaseMessageType>, IPlayerMessageReceiver<TPlayerType, TBaseMessageType> 
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
