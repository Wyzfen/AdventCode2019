using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day4
    {
        static int start = 264793;
        static int end = 803935;
        IEnumerable<string> values = Enumerable.Range(start, end - start).Select(v => v.ToString());

        [TestMethod]
        public void Problem1()
        {
            long result = values.Count(v => IsValidPassword(v));

            Assert.AreEqual(result, 966);
        }

        [TestMethod]
        public void Problem2()
        {
            long result = values.Count(v => IsValidPasswordB(v));

            Assert.AreEqual(result, 628);
        }

        private bool IsValidPassword(String input) => input.OrderBy(c => c).SequenceEqual(input) && // Is ordered
                                                      input.GroupBy(c => c).Any(g => g.Count() > 1);    // Has at least 2 same

        private bool IsValidPasswordB(String input) => input.OrderBy(c => c).SequenceEqual(input) && // Is ordered
                                                       input.GroupBy(c => c).Any(g => g.Count() == 2);   // Has set of exactly two same
    }
}
