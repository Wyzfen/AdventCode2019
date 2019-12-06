using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day6
    {
        Dictionary<string, string> map = Utils.SplitFromFile("day6.txt");

        [TestMethod]
        public void Problem1()
        {
            int result = map.Keys.Sum(k => Transfers(k).Count());

            Assert.AreEqual(result, 295834);
        }

        [TestMethod]
        public void Problem2()
        {
            var fromYou = Transfers("YOU");
            var fromSanta = Transfers("SAN");
            var common = fromYou.Intersect(fromSanta).Count();

            int result = (fromYou.Count() - common) + (fromSanta.Count() - common);

            Assert.AreEqual(result, 361);
        }

        IEnumerable<string> Transfers(string start)
        { 
            string v = map[start];
            while (v != "COM")
            {
                yield return v;
                v = map[v];
            }
            yield return v; // return COM as it's orbited too
        }
    }
}
