using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day9
    {
         long[] intCode = Utils.LongsFromCSVFile("day9.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            //intCode = new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
            //intCode = new long[] { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
            //intCode = new long[] { 104, 1125899906842624, 99 };

            long result = ShipsComputer.Compute(intCode, new List<long> { 1 }, out var outputs);

            Assert.IsNotNull(outputs);
            Assert.AreEqual(outputs.Count, 1);
            Assert.AreEqual(outputs[0], 2316632620);
        }

        [TestMethod]
        public void Problem2()
        {
            long result = ShipsComputer.Compute(intCode, new List<long> { 2 }, out var outputs);

            Assert.IsNotNull(outputs);
            Assert.AreEqual(outputs.Count, 1);
            Assert.AreEqual(outputs[0], 78869);
        }
    }
}
