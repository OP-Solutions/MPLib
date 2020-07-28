using System.Numerics;

namespace MPLib.Crypto.Encryption.SRA
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