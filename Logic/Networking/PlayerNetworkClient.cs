using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EtherBetClientLib.Crypto;
using Org.BouncyCastle.Crypto.IO;

namespace EtherBetClientLib.Networking
{
    public class PlayerNetworkClient
    {
        public string UniqueIdentifier { get; set; }
        public byte Key { get; set; }

        public IPEndPoint Endpoint { get; set; }

        public SignerStream Stream { get; private set; }
        private AesCryptoServiceProvider _aesProvider { get; set; }

        private TcpClient _client { get; set; }

        public PlayerNetworkClient(string uniqueIdentifier, IPEndPoint endpoint)
        {
            UniqueIdentifier = uniqueIdentifier;
            Endpoint = endpoint;
            _client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), endpoint.Port));
        }

        public async Task Connect()
        {
            if (!_client.Connected)
            {
                await _client.ConnectAsync(Endpoint.Address, Endpoint.Port);
                await AgreeOnAesKeyAsync();
            }
        }


        private async Task AgreeOnAesKeyAsync()
        {
            using (var keyExchange = new ECDiffieHellmanCng())
            {
                var keySizeBytes = keyExchange.KeySize / 8;
                var myPubKey = keyExchange.PublicKey.ToByteArray();
                var remotePubKey = new byte[keySizeBytes];

                var stream = _client.GetStream();
                var sendTask = stream.WriteAsync(myPubKey, 0, keySizeBytes);
                var receiveTask = stream.ReadAsync(remotePubKey, 0, keySizeBytes);
                await Task.WhenAll(sendTask, receiveTask);

                var remotePubKeyParsed = ECDiffieHellmanCngPublicKey.FromByteArray(remotePubKey, CngKeyBlobFormat.EccPublicBlob);
                var sharedKey = keyExchange.DeriveKeyFromHash(remotePubKeyParsed, HashAlgorithmName.SHA256);

                _aesProvider = new AesCryptoServiceProvider { Key = sharedKey };
                var cryptoStream = new CryptoNetWorkStream(stream, _aesProvider);
                Stream = new SignerStream(cryptoStream, null, null);
            }
        }

    }
}
