using System;
using System.Collections.Generic;
using System.Numerics;
using MPLib.Random;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class ShuffleTest
    {
        private ITestOutputHelper _output;


        [Fact]
        public void TestShuffle()
        {
            var list = new List<BigInteger>
            {
                new BigInteger(12), new BigInteger(123), new BigInteger(-123), new BigInteger(5123123)
            };
            var newList = Shuffling.Shuffle(list);
            newList.ForEach(i => _output.WriteLine(i.ToString()));
        }
    }
}