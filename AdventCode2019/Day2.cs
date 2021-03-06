﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day2
    {
        static readonly long[] intcode = new long[] {  1,  0,  0,   3,  1,  1,   2,   3, 1,   3,  4,   3, 1,   5,  0,  3,  2,  1,  6, 19,
                                                       1, 19,  5,  23,  2, 13,  23,  27, 1,  10, 27,  31, 2,   6, 31, 35,  1,  9, 35, 39,
                                                       2, 10, 39,  43,  1, 43,   9,  47, 1,  47,  9,  51, 2,  10, 51, 55,  1, 55,  9, 59,
                                                       1, 59,  5,  63,  1, 63,   6,  67, 2,   6, 67,  71, 2,  10, 71, 75,  1, 75,  5, 79,
                                                       1,  9, 79,  83,  2, 83,  10,  87, 1,  87,  6,  91, 1,  13, 91, 95,  2, 10, 95, 99,
                                                       1, 99,  6, 103,  2, 13, 103, 107, 1, 107,  2, 111, 1, 111,  9,  0, 99,  2, 14,  0,
                                                       0 };

        [TestMethod]
        public void Problem1()
        {
            long result = ShipsComputer.Compute(12, 2, intcode);

            Assert.AreEqual(result, 2692315);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    if (ShipsComputer.Compute(noun, verb, intcode) == 19690720)
                    {
                        result = 100 * noun + verb;
                        break;
                    }
                }
            }

//            result = Enumerable.Range(0, 10000).First(i => ShipsComputer.Compute(i / 100, i % 100, intcode) == 19690720);

            Assert.AreEqual(result, 9507);
        }
    }
}
