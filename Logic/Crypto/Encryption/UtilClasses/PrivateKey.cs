using System.Numerics;

namespace SPR.Crypto.Encryption.UtilClasses
{
    public struct PrivateKey
    {
        public BigInteger D { get; }
        public BigInteger N { get; }

        public PrivateKey(BigInteger d, BigInteger n)
        {
            this.D = d;
            this.N = n;
        }

    }
}