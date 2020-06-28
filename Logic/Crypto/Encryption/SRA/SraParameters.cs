using System.Numerics;
using SPR.Crypto.Encryption.UtilClasses;

namespace SPR.Crypto.Encryption.SRA
{
    public class SraParameters
    {
        public PublicKey publicKey { get; }
        public PrivateKey privateKey { get; }

        public SraParameters(PublicKey publicKey, PrivateKey privateKey)
        {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        public static SraParameters GenerateKeys()
        {
            BigInteger p, q, n, t, e, d;


            p = SecurityUtils.GetBigPrime();
            do
            {
                q = SecurityUtils.GetBigPrime();
            } while (p.Equals(q));

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

            return new SraParameters(pub, priv);

        }
    }
}
