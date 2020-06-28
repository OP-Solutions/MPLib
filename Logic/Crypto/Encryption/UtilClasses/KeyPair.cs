using System.Numerics;

namespace SPR.Crypto.Encryption.UtilClasses
{
    public class KeyPair
    {
        public PublicKey publicKey { get; }
        public PrivateKey privateKey { get; }

        public KeyPair()
        {
            privateKey = default;
            publicKey = default;
        }

        public KeyPair(PublicKey pub, PrivateKey priv)
        {
            publicKey = pub;
            privateKey = priv;
        }

        public static KeyPair GenerateKeys(BigInteger p, BigInteger q)
        {
            BigInteger n, t, e, d;

            n = BigInteger.Multiply(p, q);
            t = BigInteger.Multiply(BigInteger.Subtract(p, BigInteger.One), BigInteger.Subtract(q, BigInteger.One));
            e = BigInteger.Subtract(n, BigInteger.One);
            for (BigInteger a = e; a.CompareTo(BigInteger.Zero) > 0; a = BigInteger.Subtract(a, BigInteger.One))
            {
                if (SecurityUtils.Euclid(a, t).Equals(BigInteger.One))
                {
                    e = a;
                    break;
                }
            }
            d = SecurityUtils.Inverse(e, t);



            var pub = new PublicKey(e, n);
            var priv = new PrivateKey(d, n);

            return new KeyPair(pub, priv);

        }
    }
}