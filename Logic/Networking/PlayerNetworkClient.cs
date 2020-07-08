using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using EtherBetClientLib.Crypto;
using EtherBetClientLib.Helper;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.X509;

namespace EtherBetClientLib.Networking
{
    public class PlayerNetworkClient
    {
        public string Name { get; set; }
        public CngKey MyKey { get; }

        public CngKey OtherPartyKey { get; private set; }

        public IPEndPoint Endpoint { get; set; }

        public SignerStream Stream { get; private set; }
        private AesCryptoServiceProvider _aesProvider;

        private readonly TcpClient _client;

        public PlayerNetworkClient(string name, CngKey myKey, IPEndPoint endpoint)
        {
            Name = name;
            MyKey = myKey;
            Endpoint = endpoint;
            _client = new TcpClient(endpoint);
        }

        public async Task Connect()
        {
            if (!_client.Connected)
            {
                await _client.ConnectAsync(Endpoint.Address, Endpoint.Port);
                await AgreeOnAesKeyAsync();
                await Authentication();
            }
        }

        private async Task Authentication()
        {
            var stream = _client.GetStream();
            var controller = new StreamController(stream);
            using var randomProvider = new RNGCryptoServiceProvider();
            var myRandom = new byte[32];
            var pubKey = MyKey.Export(CngKeyBlobFormat.EccPublicBlob);
            await controller.WriteBytesOpaque16Async(pubKey);
            await controller.WriteBytesOpaque8Async(myRandom);
            var otherPartyPubKey  = await controller.ReadBytesOpaque16Async();
            var otherPartyRandom = await controller.ReadBytesOpaque8Async();
            var signer = new ECDsaCng(MyKey);
            var verifier = new ECDsaCng(CngKey.Import(otherPartyPubKey, CngKeyBlobFormat.EccFullPublicBlob));
            var mySignature = signer.SignData(otherPartyRandom, HashAlgorithmName.SHA256);
            await controller.WriteBytesOpaque16Async(mySignature);
            var otherPartySignature = await controller.ReadBytesOpaque8Async();
            verifier.VerifyData(myRandom, otherPartySignature, HashAlgorithmName.SHA256);
            await controller.WriteEnumAsByteAsync(NetWorkCodes.AuthSuccess);
            var code = await controller.ReadByteAsEnumAsync<NetWorkCodes>();
            if (code != NetWorkCodes.AuthSuccess) throw new AuthenticationException();
            OtherPartyKey = CngKey.Import(otherPartyPubKey, CngKeyBlobFormat.EccFullPublicBlob);
        }

        private async Task AgreeOnAesKeyAsync()
        {
            using var keyExchange = new ECDiffieHellmanCng();
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
