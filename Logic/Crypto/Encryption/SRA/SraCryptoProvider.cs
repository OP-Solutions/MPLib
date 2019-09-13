using System;
using BigInteger = System.Numerics.BigInteger;

namespace SPR.Crypto.Encryption.SRA
{
    public class SraCryptoProvider
    {
        public SraParameters Parameters { get; }

        public SraCryptoProvider(SraParameters parameters)
        {
            Parameters = parameters;
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
