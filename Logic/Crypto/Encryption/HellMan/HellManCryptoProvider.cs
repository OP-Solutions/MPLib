using System;
using System.Numerics;

namespace SPR.Crypto.Encryption.HellMan
{
    public class HellManCryptoProvider
    {
        public BigInteger Key { get; }

        public HellManCryptoProvider(BigInteger key)
        {
            Key = key;
        }

        public BigInteger Encrypt(BigInteger numberToEncrypt)
        {
            throw new NotImplementedException();
        }

        public BigInteger Decrypt(BigInteger numberToDecrypt)
        {
            throw new NotImplementedException();
        }
    }
}
