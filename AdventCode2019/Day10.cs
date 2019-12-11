using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day10
    {
        readonly string[] input = File.ReadAllLines("day10.txt", Encoding.UTF8);

        List<(int, int)> Parse(string[] str) => str.SelectMany((s, y) => s.Select((c, x) => c == '#' ? (x, y) : (-1, -1)).Where((a) => a.Item1 >= 0)).ToList(); 

        [TestMethod]
        public void Problem1()
        {
            int result = 0;
            //string[] test = //{ ".#..#", ".....", "#####", "....#", "...##"};
            //    { "......#.#.",
            //        "#..#.#....",
            //        "..#######.",
            //        ".#.#.###..",
            //        ".#..#.....",
            //        "..#....#.#",
            //        "#..#....#.",
            //        ".##.#..###",
            //        "##...#..#.",
            //        ".#....####" };

            var planets = Parse(input);
            var vis = planets.Select(p => CountVisible(p, planets));
            result = vis.Max();

            //System.Diagnostics.Debug.WriteLine($"Max vis @ {planets[vis.ToList().IndexOf(result)]} = {result}");

            Assert.AreEqual(result, 329);
        }

        [TestMethod]
        public void Problem2()
        {
            var source = (25, 31);

            var planets = Parse(input);
            planets.Remove(source);
            var angles = planets.Select(p => (point:p, angle:Angle(source, p))).OrderBy(a => a.angle).ThenBy(a => Distance(source, a.point)).ToList();

            int index = angles.FindIndex(a => a.angle == 0.0);
            int count = 0;
            double lastAngle = double.NaN;
            (int x, int y) target = (0, 0);

            while(count < 200)
            {
                if (index >= angles.Count()) index = 0;

                var item = angles[index];

                if (lastAngle == item.angle)
                {
                    index++;
                    continue;
                }

                lastAngle = item.angle;
                target = item.point;
                              
                angles.Remove(item); // removing item has same effect as increasing index!
                count++;

                //System.Diagnostics.Debug.WriteLine($"{count} : {item.p} @ {item.Item2}, {Distance(source, target)}");
            }


            Assert.AreEqual(target.x * 100 + target.y, 512);
        }

        (int x, int y) Delta((int x, int y) a, (int x, int y) b) => (a.x - b.x, a.y - b.y);

        private double Angle((int x, int y) start, (int x, int y) end)
        {
            return Math.Atan2(end.x - start.x, start.y - end.y); // transposed so it starts from up and goes clockwise, rather than right and anti
        }

        private int Distance((int x, int y) start, (int x, int y) end)
        {
            return (end.x - start.x) * (end.x - start.x) + (end.y - start.y) * (end.y - start.y);
        }

        private static int GCD(uint a, uint b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return (int)(a == 0 ? b : a);
        }

        (int x, int y) Simplify((int x, int y) p) 
        {
            int gcd = GCD((uint) Math.Abs(p.x), (uint) Math.Abs(p.y));
            return (p.x / gcd, p.y / gcd);
        }

        private int CountVisible((int, int) planet, List<(int, int)> planets)
        {
            return planets.Where(p => p != planet).Select(p => Simplify(Delta(planet, p))).GroupBy(p => p).Count();
        }
    }
}
