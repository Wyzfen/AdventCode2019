using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day16
    {
        readonly IEnumerable<int> values = Utils.StringsFromFile("day16.txt").First().Select(d => d - '0');

        [TestMethod]
        public void Problem1()
        {
            //string test = "80871224585914546619083218645595";
            //var values = values.Select(d => d - '0');

            IEnumerable<int> input = values;
            var output = FFT(input);

            int result = output.Take(8).ToInt();

            Assert.AreEqual(result, 11833188);
        }


        [TestMethod]
        public void Problem2()
        {
            IEnumerable<int> input = Enumerable.Repeat(values, 10000).SelectMany(e => e);
            var output = FFT(input);

            int offset = input.Take(7).ToInt();

            int result = output.Skip(offset).Take(8).ToInt();

            Assert.AreEqual(result, 4825810);
        }

        private static IEnumerable<int> FFT(IEnumerable<int> input)
        {
            var patternBase = new int[] { 0, 1, 0, -1 };

            int count = input.Count();
            int[] output = new int[count];
            for (int phase = 0; phase < 100; phase++)
            {
                for (int i = 0; i < count; i++)
                {
                    int n = (count + 4 * (i + 1)) / (4 * (i + 1));
                    var pattern = Enumerable.Repeat(patternBase.SelectMany(p => Enumerable.Repeat(p, i + 1)), n).SelectMany(e => e).Skip(1);

                    output[i] = Math.Abs(input.Skip(i).Zip(pattern.Skip(i), (inp, p) => inp * p).Sum()) % 10;
                }

                input = output;
            }

            return output;
        }
    }
}
