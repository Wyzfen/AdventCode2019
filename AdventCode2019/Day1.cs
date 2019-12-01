using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day1
    {
        IEnumerable<int> values = Utils.IntsFromFile("day1.txt");

        [TestMethod]
        public void Problem1()
        {
            int result = values.Select(v => Calc(v)).Sum();

            Assert.AreEqual(result, 3219099);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = values.Select(v => CalcRecurse(v)).Sum();

            Assert.AreEqual(result, 4825810);
        }

        private int Calc(int value) => value / 3 - 2;

        private int CalcRecurse(int value)
        {
            int total = 0;

            while ((value = Calc(value)) > 0)
            {
                total += value;
            } 
            
            return total;
        }
    }
}
