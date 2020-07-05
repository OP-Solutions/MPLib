using System.Numerics;

namespace EtherBetClientLib.Crypto.Encryption.SRA
{
    public struct PublicKey
    {
        public BigInteger E { get; }
        public BigInteger N { get; }

        public PublicKey(BigInteger e, BigInteger n)
        {
            this.E = e;
            this.N = n;
        }
    }
}