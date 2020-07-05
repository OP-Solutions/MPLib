using System.Numerics;
using EtherBetClientLib.Crypto.Encryption.UtilClasses;

namespace EtherBetClientLib.Crypto.Encryption.HellMan
{
    public class PohligHellmanKey
    {
        public BigInteger E { get; }
        public BigInteger D { get; }
        public BigInteger P { get; }

        public PohligHellmanKey()
        {
            E = BigInteger.Zero;
            D = BigInteger.Zero;
            P = BigInteger.Zero;
        }
        public PohligHellmanKey(BigInteger e, BigInteger d, BigInteger p)
        {
            E = e;
            D = d;
            P = p;
        }

        public static PohligHellmanKey GenerateKey()
        {
            var p = SecurityUtils.GetBigPrime();
            //get e,d;
            var e = BigInteger.MinusOne;
            var d = SecurityUtils.Inverse(e, BigInteger.Subtract(p, BigInteger.One));

            return new PohligHellmanKey(e, d, p);
        }

    }
}