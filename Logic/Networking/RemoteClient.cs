using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Nethereum.Signer;
using SPR.Crypto;

namespace SPR.Networking
{
    public class RemoteClient
    {
        public string EthAddress { get; set; }
        public EthECKey EcKey { get; set; }

        public IPEndPoint Endpoint { get; set; }

        public AesCryptoServiceProvider AesProvider { get; set; }

        private TcpClient _client { get; set; }

        public RemoteClient(string ethAddress, EthECKey ecKey, IPEndPoint endpoint )
        {
            EthAddress = ethAddress;
            EcKey = ecKey;
            Endpoint = endpoint;
            _client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), endpoint.Port));
        }

        public async Task Connect()
        {
            if (!_client.Connected)
            {
                await _client.ConnectAsync(Endpoint.Address, Endpoint.Port);
                ExchangeAesKeys();
            }

        }

        public CryptoNetWorkStream GetStream()
        {
            return new CryptoNetWorkStream(_client.Client, AesProvider);
        }

        private void ExchangeAesKeys()
        {
            var provider = new AesCryptoServiceProvider();
            provider.GenerateKey();
            var localKey = provider.Key;
            _client.Client.Send(localKey);
            var remoteKey = new byte[32];
            var toReceive = 32;
            while (toReceive > 0)
            {
                toReceive -= _client.Client.Receive(remoteKey);
            }

            provider.Key = Xor(localKey, remoteKey);
            AesProvider = provider;
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
