using System.Collections.Generic;
using System.Numerics;

namespace EtherBetClientLib.Random
{
    public class Shuffling
    {
        /// <summary>
        ///     Simple Method to shuffle list.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<BigInteger> Shuffle(IEnumerable<BigInteger> source)
        {
            var list = new List<BigInteger>();
            list.AddRange(source);

            for (var i = list.Count - 1; i > 0; i--)
            {
                var rand = SafeRandomGenerator.GetInt(0, i);

                var temp = list[i];
                list[i] = list[rand];
                list[rand] = temp;
            }

            return list;
        }
    }
}