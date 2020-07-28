using System;
using System.Collections.Generic;
using System.Numerics;
using MPLib.Random;
using Xunit;

namespace SPRTest
{
    public class ShuffleTest
    {
        [Fact]
        public void TestMethod1()
        {
            var list = new List<BigInteger>
            {
                new BigInteger(12), new BigInteger(123), new BigInteger(-123), new BigInteger(5123123)
            };
            var newList = Shuffling.Shuffle(list);
            newList.ForEach(i => Console.Write("{0}\t", i));
        }
    }
}