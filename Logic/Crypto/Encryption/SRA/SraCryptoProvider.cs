using System;
using System.Text;
using SPR.Crypto.Encryption.UtilClasses;
using BigInteger = System.Numerics.BigInteger;
using System.Security.Cryptography;

namespace SPR.Crypto.Encryption.SRA
{
    public class SraCryptoProvider
    {
        private SraParameters keyPair { get; }

        public SraCryptoProvider(SraParameters keyPair)
        {
            this.keyPair = keyPair;
        }

        public BigInteger Decrypt(BigInteger m)
        {
            var key = keyPair.privateKey;
            var decrypted = BigInteger.ModPow(m, key.D, key.N);
            return decrypted;

        }

        public BigInteger Encrypt(BigInteger m)
        {
            var key = keyPair.publicKey;
            var encrypted = BigInteger.ModPow(m, key.E, key.N);
            return encrypted;
        }
    }
}
