using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPR.Core.Random;

namespace SPRTest
{
    [TestClass]
    public class ShuffleTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = new List<BigInteger>
            {
                new BigInteger(12), new BigInteger(123), new BigInteger(-123), new BigInteger(5123123)
            };
            var newList = new Shuffling().Shuffle(list);
            newList.ForEach(i => Console.Write("{0}\t", i));
        }
    }
}