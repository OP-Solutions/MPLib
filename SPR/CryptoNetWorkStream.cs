using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SPR.Models;

namespace SPR
{
    public class CryptoNetWorkStream
    {
        private Socket _socket;

        private ICryptoTransform _encryptor;
        private ICryptoTransform _decryptor;

        public CryptoNetWorkStream(Socket socket, AesCryptoServiceProvider aes)
        {
            _socket = socket;
            _encryptor = aes.CreateEncryptor();
            _decryptor = aes.CreateDecryptor();
        }

        public void SendMessage(object message)
        {
            using (var aes = new System.Security.Cryptography.CryptoStream(new NetworkStream(_socket, FileAccess.ReadWrite), _encryptor, CryptoStreamMode.Write))
            {
                using (var writer = new StreamWriter(aes, Encoding.UTF8))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, message);
                }
            }
        }

        public object ReceiveMessage()
        {
            using (var aes = new System.Security.Cryptography.CryptoStream(new NetworkStream(_socket, FileAccess.ReadWrite), _decryptor, CryptoStreamMode.Read))
            {
                using (var reader = new StreamReader(aes, Encoding.UTF8))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        var serializer = new JsonSerializer();
                        return serializer.Deserialize(jsonReader);
                    }
                    
                }
            }
        }
    }
}
