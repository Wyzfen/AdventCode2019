using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day9
    {
        readonly int[] intCode = Utils.IntsFromCSVFile("day7.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            int result = ShipsComputer.Compute(intCode, null, out var outputs);

            Assert.IsNotNull(outputs);
            Assert.AreNotEqual(outputs.Count, 0);
            Assert.AreEqual(outputs[0], 3219099);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            Assert.AreEqual(result, 4825810);
        }
    }
}
