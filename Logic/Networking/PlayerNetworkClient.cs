using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Signer;
using Org.BouncyCastle.Crypto.Tls;
using SPR.Crypto;
using SPR.Helper;

namespace SPR.Networking
{
    public class PlayerNetworkClient
    {
        public string EthAddress { get; set; }
        public EthECKey EcKey { get; set; }

        public IPEndPoint Endpoint { get; set; }

        public CryptoNetWorkStream Stream { get; private set; }
        private AesCryptoServiceProvider _aesProvider { get; set; }

        private TcpClient _client { get; set; }

        public PlayerNetworkClient(string ethAddress, IPEndPoint endpoint)
        {
            EthAddress = ethAddress;
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

        private async Task AuthenticateAsync()
        {
            var myRandom = new byte[32];
            using (var randGen = new RNGCryptoServiceProvider())
            {
                randGen.GetBytes(myRandom);
                await Stream.WriteAsync(myRandom, 0, myRandom.Length);
            }
        }

        private async Task AgreeOnAesKeyAsync()
        {
            using (var keyExchange = new ECDiffieHellmanCng())
            {
                var keySizeBytes = keyExchange.KeySize / 8;
                keyExchange.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                keyExchange.HashAlgorithm = CngAlgorithm.Sha256;
                var myPubKey = keyExchange.PublicKey.ToByteArray();
                var remotePubKey = new byte[keySizeBytes];

                using (var stream = _client.GetStream())
                {
                    var sendTask = stream.WriteAsync(myPubKey, 0, keySizeBytes);
                    var receiveTask = stream.ReadAsync(remotePubKey, 0, keySizeBytes);
                    await Task.WhenAll(sendTask, receiveTask);
                }

                var sharedKey = keyExchange.DeriveKeyMaterial(CngKey.Import(remotePubKey, CngKeyBlobFormat.EccPublicBlob));
                _aesProvider = new AesCryptoServiceProvider { Key = sharedKey };
                Stream = new CryptoNetWorkStream(_client.GetStream(), _aesProvider);
            }
        }

        private static byte[] Xor(IReadOnlyList<byte> bytes1, IReadOnlyList<byte> bytes2)
        {
            var result = new byte[bytes1.Count];
            for (var i = 0; i < bytes1.Count; i++)
            {
                var b1 = bytes1[i];
                var b2 = bytes2[i];
                result[i] = (byte)(b1 ^ b2);
            }

            return result;
        }

    }
}
