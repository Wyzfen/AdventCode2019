using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;

namespace AdventCode2019
{
    [TestClass]
    public class Day17
    {
        readonly long [] intCode = Utils.LongsFromCSVFile("day17.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            ShipsComputer robot = new ShipsComputer(intCode);
            long[] output = robot.Execute().ToArray();
            string video = new string(Array.ConvertAll(output, l => (char)l));
            string[] lines = video.Split(new char[] { (char) 10 });

            long result = 0;
            for (int y = 1; y < lines.Length - 1; y++)
            {
                for (int x = 1; x < lines[y].Length - 1; x++)
                {
                    if(lines[y][x] != '.' && lines[y][x-1] != '.' && lines[y][x + 1] != '.' &&
                        lines[y - 1][x] != '.' && lines[y + 1][x] != '.')
                    {
                        Debug.WriteLine($"{x}, {y}");
                        result += (x * y);
                    }
                }
            }

            Assert.AreEqual(result, 6212);
        }

        [TestMethod]
        public void Problem2()
        {
            intCode[0] = 2;
            ShipsComputer robot = new ShipsComputer(intCode);

            // Note: Call toarray after execute to consume output -> wont complete execute otherwise
            robot.Execute("A,A,B,C,B,C,B,C,C,A\n").ToArray();
            robot.Execute("L,10,R,8,R,8\n").ToArray();
            robot.Execute("L,10,L,12,R,8,R,10\n").ToArray();
            robot.Execute("R,10,L,12,R,10\n").ToArray();
            var output = robot.Execute("n\n").ToArray();

            long result = output.Last();

            Assert.AreEqual(result, 1016741);
        }
    }
}
