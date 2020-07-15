using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class MessageManager<TBaseMessageType> : IMessageManager<TBaseMessageType> where TBaseMessageType : IMessage
    {
        /// <summary>
        /// Source identifier to send along with messages. (so remote party knows where messages are coming from)
        /// </summary>
        private readonly string _sourceIdentifierToSend;

        /// <summary>
        /// Destination Identifier to send along with messages. (so remote party knows where messages are coming from)
        /// all players act as proxy nodes also, they should relay message to 3rd player if they are not "destination" itself. this is why that property is needed
        /// </summary>
        private readonly string _destinationIdentifierToSend;

        /// <summary>
        /// ECC public and private key to sign messages with ECDSA
        /// </summary>
        private CngKey _localKey;

        /// <summary>
        /// ECC public key to verify remote party's singed messages
        /// </summary>
        private CngKey _remoteExceptedPublicKey;

        private readonly Stream _targetStream;

        private readonly ECDsa _signer;

        public MessageManager(string sourceIdentifierToSend, string destinationIdentifierToSend, CngKey localKey, CngKey remoteExceptedPublicKey, Stream targetStream)
        {
            _sourceIdentifierToSend = sourceIdentifierToSend;
            _destinationIdentifierToSend = destinationIdentifierToSend;
            _localKey = localKey;
            _remoteExceptedPublicKey = remoteExceptedPublicKey;
            _targetStream = targetStream;
            _signer = new ECDsaCng(localKey)
            {
                HashAlgorithm = CngAlgorithm.Sha256,
            };
        }

        public async Task SendMessage(TBaseMessageType message)
        {
            var package = new Package()
            {
                SenderIdentifier = _sourceIdentifierToSend,
                DestinationIdentifier = _destinationIdentifierToSend,
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
                await stream.CopyToAsync(_targetStream);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            
        }

        public async Task<T> ReadMessage<T>() where T : TBaseMessageType
        {
            var buffer = ArrayPool<byte>.Shared.Rent(1024 * 32);
            try
            {
                var controller = new StreamController(_targetStream);
                var dataLen = await controller.ReadBytesOpaque16Async(buffer, 0);
                var signature = _signer.SignData(buffer, 0 , dataLen, HashAlgorithmName.SHA256);
                if(!_signer.VerifyData(buffer, 0, dataLen, signature, HashAlgorithmName.SHA256)) throw new Exception();
                var package = Serializer.Deserialize<Package>(new ReadOnlySpan<byte>(buffer, 0, dataLen));
                package.Signature = signature;
                return (T)package.Message;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }

    public interface IMessageManager<in TBaseMessageType> : IMessageSender<TBaseMessageType>, IMessageReceiver<TBaseMessageType> where TBaseMessageType : IMessage
    {
    }

    public interface IMessageSender<in TBaseMessageType> where TBaseMessageType : IMessage
    {
        Task SendMessage(TBaseMessageType message);
    }

    public interface IMessageReceiver<in TBaseMessageType> where TBaseMessageType : IMessage
    {
        Task<T> ReadMessage<T>() where T : TBaseMessageType;
    }


    public class BroadCastMessageSender<TBaseMessageType> : IMessageSender<TBaseMessageType> where TBaseMessageType : IMessage
    {

        private readonly IMessageSender<TBaseMessageType>[] _targetManagers;

        public BroadCastMessageSender(IMessageSender<TBaseMessageType>[] targetManagers)
        {
            _targetManagers = targetManagers;
        }

        public async Task SendMessage(TBaseMessageType message)
        {
            var tasks = _targetManagers.Select(s => s.SendMessage(message));
            await Task.WhenAll(tasks);
        }
    }
}
