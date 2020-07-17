using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using EtherBetClientLib.Core.Game.Poker.Messaging;
using EtherBetClientLib.Helper;
using EtherBetClientLib.Models;
using EtherBetClientLib.Models.Games;
using ProtoBuf;

namespace EtherBetClientLib.Networking
{
    public class PlayerMessageManager<TPlayerType, TBaseMessageType> : IPlayerMessageManager<TPlayerType, TBaseMessageType> where TBaseMessageType : IMessage where TPlayerType : Player
    {

        private readonly TPlayerType _myPlayer;
        private readonly string _myIdentifier;
        private readonly ECDsa _signer;
        private readonly IReadOnlyDictionary<TPlayerType, string> _connectedPlayers;


        /// <param name="myPlayer"></param>
        /// <param name="myIdentifier">identifier to include in messages, so receiver know that it come from this player</param>
        /// <param name="otherPlayers">Other players dictionary. key is player itself, value - corresponding player identifier</param>
        public PlayerMessageManager(TPlayerType myPlayer, string myIdentifier, IReadOnlyDictionary<TPlayerType, string> otherPlayers)
        {
            _connectedPlayers = otherPlayers;
            _myIdentifier = myIdentifier;
            _myPlayer = myPlayer;
            _signer = new ECDsaCng(_myPlayer.Key);
        }

        public async Task SendMessageTo(TPlayerType player, TBaseMessageType message)
        {
            var package = new Package<TBaseMessageType>()
            {
                SenderIdentifier = _myIdentifier,
                DestinationIdentifier = _connectedPlayers[player],
                Message = message
            };

            var buffer = ArrayPool<byte>.Shared.Rent(1024 * 32);
            try
            {
                await using var stream = new MemoryStream(buffer) { Position = 2 };
                Serializer.Serialize(stream, package);
                var pos = (int)stream.Position;
                var len = pos - 2;
                buffer[0] = (byte)len;
                buffer[1] = (byte)(len >> 8);
                var signature = _signer.SignData(buffer, 2, len, HashAlgorithmName.SHA256);
                stream.Position += 2;
                var signatureLen = signature.Length;
                buffer[pos] = (byte)signatureLen;
                buffer[pos + 1] = (byte)(signatureLen >> 8);
                await stream.CopyToAsync(player.NetworkStream);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            
        }

        public async Task<T> ReadMessageFrom<T>(TPlayerType player) where T : TBaseMessageType
        {
            var buffer = ArrayPool<byte>.Shared.Rent(1024 * 32);
            try
            {
                var controller = new StreamController(player.NetworkStream);
                var dataLen = await controller.ReadBytesOpaque16Async(buffer, 0);
                var signature = _signer.SignData(buffer, 0 , dataLen, HashAlgorithmName.SHA256);
                if(!_signer.VerifyData(buffer, 0, dataLen, signature, HashAlgorithmName.SHA256)) throw new Exception();
                var package = Serializer.Deserialize<Package<TBaseMessageType>>(new ReadOnlySpan<byte>(buffer, 0, dataLen));
                package.Signature = signature;
                return (T)package.Message;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
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
    }

    public interface IPlayerMessageReceiver<in TPlayerType, in TBaseMessageType> where TBaseMessageType : IMessage
    {
        Task<T> ReadMessageFrom<T>(TPlayerType player) where T : TBaseMessageType;
    }
}
