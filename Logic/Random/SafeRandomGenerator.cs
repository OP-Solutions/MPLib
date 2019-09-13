using System;
using System.Security.Cryptography;

namespace SPR.Random
{
    public static class SafeRandomGenerator
    {
        /// <summary>
        ///     returns random integer
        /// </summary>
        /// <returns></returns>
        public static int GetInt()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[4];
                provider.GetBytes(byteArray);

                var randomInteger = BitConverter.ToUInt32(byteArray, 0);
                return (int) randomInteger;
            }
        }

        /// <summary>
        ///     returns random integer from <paramref name="minValue" /> to <paramref name="maxValue" />
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int GetInt(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException(
                    $"{nameof(minValue)} should be less or equal to {nameof(maxValue)}");

            if (minValue == maxValue) return minValue;

            long cnt = maxValue - minValue + 1;
            var rand = Math.Abs(GetInt()) % cnt;
            return minValue + (int) rand;
        }

        /// <summary>
        ///     returns random integer bigger than <paramref name="minValue" />
        /// </summary>
        /// <param name="minValue"></param>
        /// <returns></returns>
        public static int GetInt(int minValue)
        {
            return GetInt(minValue, int.MaxValue);
        }
    }
}