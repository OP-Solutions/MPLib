using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AppContext;
using Nethereum.RLP;
using Nethereum.Signer;

namespace SPR.Models
{
    public class RemoteClient
    {
        public string EthAddress { get; set; }
        public EthECKey EcKey { get; set; }

        public IPEndPoint Endpoint { get; set; }

        public AesCryptoServiceProvider AesProvider { get; set; }

        private TcpClient _client { get; set; }

        public RemoteClient(string ethAddress, EthECKey ecKey, IPEndPoint endpoint)
        {
            EthAddress = ethAddress;
            EcKey = ecKey;
            Endpoint = endpoint;
        }

        public void Connect()
        {
            if (!_client.Connected)
            {
                _client.Connect(Endpoint);
                ExchangeAesKeys();
            }

        }

        public CryptoStream GetStream()
        {
            return new CryptoStream(_client.GetStream(), AesProvider);
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

        private byte[] Xor(byte[] bytes1, byte[] bytes2)
        {
            var result = new byte[bytes1.Length];
            for (var i = 0; i < bytes1.Length; i++)
            {
                var b1 = bytes1[i];
                var b2 = bytes2[i];
                result[i] = (byte)(b1 ^ b2);
            }

            return result;
        }

    }
}
