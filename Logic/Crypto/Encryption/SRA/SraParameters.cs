using System.Numerics;
using EtherBetClientLib.Crypto.Encryption.UtilClasses;

namespace EtherBetClientLib.Crypto.Encryption.SRA
{
    public class SraParameters
    {
        public BigInteger EncryptKey { get; }
        public BigInteger DecryptKey { get; }

        public BigInteger Modulus { get; }

        public SraParameters(BigInteger encryptKey, BigInteger decryptKey, BigInteger modulus)
        {
            this.EncryptKey = encryptKey;
            this.DecryptKey = decryptKey;
            this.Modulus = modulus;
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
            for (var a = e; a.CompareTo(BigInteger.Zero) > 0; a = BigInteger.Subtract(a, BigInteger.One))
            {
                if (SecurityUtils.Euclid(a, t).Equals(BigInteger.One))
                {
                    e = a;
                    break;
                }
            }
            d = SecurityUtils.Inverse(e, t);
            return new SraParameters(e, d, n);

        }
    }
}
