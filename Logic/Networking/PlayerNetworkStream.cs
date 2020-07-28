using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MPLib.Crypto;
using MPLib.Helper;
using MPLib.Models.Games;

namespace MPLib.Networking
{
    public class PlayerNetworkStream
    {
        public Player MyPlayer { get; set; }


        public Player RemotePlayer { get; set; }

        /// <summary>
        /// Remote player's endpoint
        /// </summary>
        public IPEndPoint Endpoint { get; set; }

        public Stream Stream { get; private set; }
        private AesCryptoServiceProvider _aesProvider;

        private readonly TcpClient _client;

        public PlayerNetworkStream(Player myPlayer, IPEndPoint endpointToConnect)
        {
            MyPlayer = myPlayer;
            Endpoint = endpointToConnect;
            _client = new TcpClient(endpointToConnect);
        }

        public async Task<Player> Connect()
        {
            if (!_client.Connected)
            {
                await _client.ConnectAsync(Endpoint.Address, Endpoint.Port);
                await AgreeOnAesKeyAsync();
                await Authentication();
                await ExchangeBasicInfo();
            }

            return RemotePlayer;
        }


        private async Task ExchangeBasicInfo()
        {
            var stream = _client.GetStream();
            await stream.WriteAsciiOpaque8Async(MyPlayer.Name);
            var remotePlayerName = await stream.ReadAsciiOpaque8Async();
            RemotePlayer.Name = remotePlayerName;
        }

        private async Task Authentication()
        {
            var stream = _client.GetStream();
            using var randomProvider = new RNGCryptoServiceProvider();
            var myRandom = new byte[32];
            var pubKey = MyPlayer.Key.Export(CngKeyBlobFormat.EccFullPublicBlob);
            await stream.WriteBytesOpaque16Async(pubKey);
            await stream.WriteBytesOpaque8Async(myRandom);
            var otherPartyPubKey  = await stream.ReadBytesOpaque16Async();
            var otherPartyRandom = await stream.ReadBytesOpaque8Async();
            var signer = new ECDsaCng(MyPlayer.Key);
            var remotePlayerKey = CngKey.Import(otherPartyPubKey, CngKeyBlobFormat.EccPublicBlob);
            var verifier = new ECDsaCng(remotePlayerKey);
            var mySignature = signer.SignData(otherPartyRandom, HashAlgorithmName.SHA256);
            await stream.WriteBytesOpaque16Async(mySignature);
            var otherPartySignature = await stream.ReadBytesOpaque8Async();
            verifier.VerifyData(myRandom, otherPartySignature, HashAlgorithmName.SHA256);
            await stream.WriteEnumAsByteAsync(NetWorkCodes.AuthSuccess);
            var code = await stream.ReadByteAsEnumAsync<NetWorkCodes>();
            if (code != NetWorkCodes.AuthSuccess) throw new AuthenticationException();
            RemotePlayer.Key = remotePlayerKey;
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
            Stream = cryptoStream;
        }

    }
}
