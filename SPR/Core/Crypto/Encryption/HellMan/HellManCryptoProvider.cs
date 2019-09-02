using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Core.Crypto.Encryption.HellMan
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
