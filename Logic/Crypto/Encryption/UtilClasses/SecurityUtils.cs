using System.Numerics;

namespace SPR.Crypto.Encryption.UtilClasses
{
    public static class SecurityUtils
    {
        public static BigInteger GetBigPrime()
        {
            return BigInteger.MinusOne;
        }

        public static BigInteger Inverse(BigInteger a, BigInteger n)
        {
            BigInteger[] bigInts = ExtendedEuclid(a, n);
            if (!bigInts[0].Equals(BigInteger.One))
            {
                return BigInteger.MinusOne;
            }

            if (bigInts[1].CompareTo(BigInteger.Zero) == 1)
            {
                return bigInts[1];
            }
            else
                return BigInteger.Add(bigInts[1], n);
        }

        public static BigInteger Euclid(BigInteger a, BigInteger b)
        {
            return b.Equals(BigInteger.Zero) ? a : Euclid(b, BigInteger.Remainder(a, b));
        }
        public static BigInteger[] ExtendedEuclid(BigInteger a, BigInteger b)
        {
            BigInteger x, y;
            BigInteger[] bigInts = new BigInteger[3];

            if (b.Equals(BigInteger.Zero))
            {
                bigInts[0] = a;
                bigInts[1] = BigInteger.One;
                bigInts[2] = BigInteger.Zero;
                return bigInts;
            }
            bigInts = ExtendedEuclid(b, BigInteger.Remainder(a, b));

            x = bigInts[1];
            y = bigInts[2];
            bigInts[1] = y;
            bigInts[2] = BigInteger.Subtract(x, BigInteger.Multiply(y, BigInteger.Divide(a, b)));

            return bigInts;
        }


    }
}