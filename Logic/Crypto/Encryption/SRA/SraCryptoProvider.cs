using System;
using System.Text;
using SPR.Crypto.Encryption.UtilClasses;
using BigInteger = System.Numerics.BigInteger;
using System.Security.Cryptography;

namespace SPR.Crypto.Encryption.SRA
{
    public class SraCryptoProvider
    {
        private KeyPair keyPair { get; }

        public SraCryptoProvider(KeyPair keyPair)
        {
            this.keyPair = keyPair;
        }

        public string Decrypt(string m)
        {
            var msg = Encoding.ASCII.GetBytes(m);
            var key = keyPair.privateKey;

            var encrypted = new BigInteger(msg);
            var message = BigInteger.ModPow(encrypted, key.D, key.N);
            return Encoding.ASCII.GetString(message.ToByteArray());

        }

        public string Encrypt(String m)
        {

            var key = keyPair.publicKey;
            var msg = Encoding.ASCII.GetBytes(m);
            var message = new BigInteger(msg);
            var encrypted = BigInteger.ModPow(message, key.E, key.N);

            return Encoding.ASCII.GetString(encrypted.ToByteArray());
        }
    }
}
