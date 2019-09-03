using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SPR.Core.Random
{
    public class Shuffling
    {
        /// <summary>
        /// Simple Method to shuffle list.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<BigInteger> Shuffle(List<BigInteger> source)
        {
            var rnd = new System.Random();
            return source.OrderBy(x => rnd.Next()).ToList();
        }
    }
}