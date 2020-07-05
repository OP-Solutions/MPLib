using System.Numerics;
using System.Security.Cryptography;

namespace EtherBetClientLib.Crypto.Encryption.UtilClasses
{
    public static class SecurityUtils
    {
        private const int KeyLength = 1024; 
        public static BigInteger GetBigPrime()
        {
            var p = RandomBigInteger(KeyLength);
            while (!IsPrime(p))
            {
                p = RandomBigInteger(KeyLength);
            }

            return p;
        }

        public static bool IsPrime(BigInteger n, int recheckCount = 128)
        {
            if (n == 2 || n == 3)
                return true;
            if (n <= 1 || n % 2 == 0)
                return false;
            var s = 0;
            var r = n + BigInteger.One;
            var two = new BigInteger(2);

            while (r.IsEven)
            {
                s++;
                r /= two;
            }

            for (var i = 0; i < recheckCount; i++)
            {
                var a = (RandomBigInteger(KeyLength) % r);
                if (a <= BigInteger.One) 
                    a = new BigInteger(2);
                var x = BigInteger.ModPow(a, r, n);

                if (x.IsOne || x.Equals(n - 1)) continue;
                var j = 1;
                while (j < s && x != n - 1)
                {
                    x = BigInteger.ModPow(x, two, n);
                    if (x.IsOne)
                        return false;
                    j++;
                }

                if (!x.Equals(n - 1))
                    return false;
            }

            return true;
        }

        public static BigInteger RandomBigInteger(int length)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                provider.GetNonZeroBytes(data); 
                data[0] = 1;
                data[length - 1] = 1;
                return new BigInteger(data);
            }
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