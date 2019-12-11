using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day11
    {
        readonly long[] intCode = Utils.LongsFromCSVFile("day11.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            int result = PaintingRobot(0, new ShipsComputer(intCode)).Keys.Count();

            Assert.AreEqual(result, 2041);
        }

        [TestMethod]
        public void Problem2()
        {
            var output = PaintingRobot(1, new ShipsComputer(intCode));

            int X = output.Keys.Max(k => k.x);
            int Y = output.Keys.Max(k => k.y);

            StringBuilder[] image = Enumerable.Range(0, Y + 1).Select(_ => new StringBuilder(new string(' ', X + 1))).ToArray();
            char[] pixels = { '·', '█', ' ' };

            foreach(var item in output)
            {
                image[item.Key.y][item.Key.x] = pixels[item.Value];
            }

            foreach (var line in image)
            {
                System.Diagnostics.Debug.WriteLine(line);
            }

            Assert.AreEqual(image[0].ToString(), "·████·███··████·███··█··█·████·████·███··· ");
            Assert.AreEqual(image[1].ToString(), " ···█·█··█····█·█··█·█·█··█·······█·█··█···");
            Assert.AreEqual(image[2].ToString(), " ··█··█··█···█··█··█·██···███····█··█··█···");
            Assert.AreEqual(image[3].ToString(), "··█···███···█···███··█·█··█·····█···███··· ");
            Assert.AreEqual(image[4].ToString(), "·█····█·█··█····█····█·█··█····█····█·█··  ");
            Assert.AreEqual(image[5].ToString(), " ████·█··█·████·█····█··█·████·████·█··█·  ");
        }

        Dictionary<(int x, int y), int> PaintingRobot(int startColour, ShipsComputer computer)
        {
            // List of where has painted. Note; assumed will paint first square for convenience.
            var painted = new Dictionary<(int x, int y), int> { { (0, 0), startColour } };

            int x = 0;
            int y = 0;
            int heading = 0; // 0 = up, 1 = right etc;

            while(!computer.Completed)
            {
                painted.TryGetValue((x, y), out int input);
                var output = computer.Execute(input).ToList();

                painted[(x, y)] = (int) output[0];
                heading = (heading + (output[1] == 0 ? -1 : 1) + 4) % 4;
                
                switch(heading)
                {
                    case 0: y--; break;
                    case 1: x++; break;
                    case 2: y++; break;
                    case 3: x--; break;
                }
            }

            return painted;
        }
    }
}
