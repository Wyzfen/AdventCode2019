using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;

namespace AdventCode2019
{
    [TestClass]
    public class Day21
    {
        readonly long [] intCode = Utils.LongsFromCSVFile("day21.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            ShipsComputer robot = new ShipsComputer(intCode);

            var input = String.Join("\n", new string[] { "NOT A J",
                                        "NOT B T",
                                        "OR T J",
                                        "NOT C T",
                                        "OR T J",
                                        "AND D J",
                                        "WALK\n" });

            long[] output = robot.Execute(input).ToArray();
            long result = output.Last();

            //string video = new string(Array.ConvertAll(output, l => (char)l));
            //string[] lines = video.Split(new char[] { (char)10 });
            //foreach (var line in lines)
            //{
            //    Debug.WriteLine(line);
            //}

            Assert.AreEqual(result, 19355391);
        }

        [TestMethod]
        public void Problem2()
        {
            ShipsComputer robot = new ShipsComputer(intCode);

            var input = String.Join("\n", new string[] { "NOT A J",
                                        "NOT B T",
                                        "OR T J",
                                        "NOT C T",
                                        "OR T J",
                                        "AND D J",
                                        "NOT E T", // double not E to get E into T (cant rely on state of T)
                                        "NOT T T",
                                        "OR H T",
                                        "AND T J",
                                        "RUN\n" });

            // a | b | c & d & (h | e)

            long[] output = robot.Execute(input).ToArray();
            long result = output.Last();

            //string video = new string(Array.ConvertAll(output, l => (char)l));
            //string[] lines = video.Split(new char[] { (char)10 });
            //foreach (var line in lines)
            //{
            //    Debug.WriteLine(line);
            //}

            Assert.AreEqual(result, 1143770635);
        }
    }
}
