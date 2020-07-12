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
using Org.BouncyCastle.Crypto.Signers;
using ProtoBuf;

namespace EtherBetClientLib.Networking
{
    public class MessageManager : IMessageManager
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

        private ECDsa _signer;

        public MessageManager(string sourceIdentifierToSend, string destinationIdentifierToSend, CngKey localKey, CngKey remoteExceptedPublicKey, Stream targetStream)
        {
            _sourceIdentifierToSend = sourceIdentifierToSend;
            _destinationIdentifierToSend = destinationIdentifierToSend;
            _localKey = localKey;
            _remoteExceptedPublicKey = remoteExceptedPublicKey;
            _targetStream = targetStream;
            _signer = new ECDsaCng()
            {
                HashAlgorithm = CngAlgorithm.Sha256,
            };
        }

        public async Task SendMessage(IMessage message)
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
                await using var stream = new MemoryStream(buffer);
                var controller = new StreamController(stream);
                Serializer.Serialize(stream, package);
                var signature = _signer.SignData(stream.GetBuffer(), 0, (int)stream.Position, HashAlgorithmName.SHA256);
                await controller.WriteBytesOpaque16Async(signature);
                await stream.CopyToAsync(_targetStream);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
            
        }

        public async Task<Message> ReadMessage()
        {
            throw new NotImplementedException();
        }
    }

    public interface IMessageManager
    {
        Task SendMessage(IMessage message);
    }


    public class BroadCastMessageManager : IMessageManager
    {

        private readonly IMessageManager[] _targetSenders;

        public BroadCastMessageManager(IMessageManager[] targetSenders)
        {
            _targetSenders = targetSenders;
        }

        public async Task SendMessage(IMessage message)
        {
            var tasks = _targetSenders.Select(s => s.SendMessage(message));
            await Task.WhenAll(tasks);
        }
    }
}
