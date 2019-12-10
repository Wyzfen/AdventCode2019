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

            System.Diagnostics.Debug.WriteLine($"Max vis @ {planets[vis.ToList().IndexOf(result)]} = {result}");

            Assert.AreEqual(result, 329);
        }

        [TestMethod]
        public void Problem2()
        {
            var source = (25, 31);

            var planets = Parse(input);
            planets.Remove(source);
            var angles = planets.Select(p => (p, Angle(source, p))).OrderBy(a => a.Item2).ThenBy(a => Distance(source, a.p)).ToList();

            int index = angles.FindIndex(a => a.Item2 == 0.0);
            int count = 0;
            double lastAngle = double.NaN;
            (int x, int y) target = (0, 0);

            while(count < 200)
            {
                if (index >= angles.Count()) index = 0;

                var item = angles[index];

                if (lastAngle == item.Item2)
                {
                    index++;
                    continue;
                }

                lastAngle = item.Item2;
                target = item.p;
                              
                angles.Remove(item); // removing item has same effect as increasing index!
                count++;

                System.Diagnostics.Debug.WriteLine($"{count} : {item.p} @ {item.Item2}, {Distance(source, target)}");
            }


            Assert.AreEqual(target.x * 100 + target.y, 512);
        }

        (int x, int y) Delta((int x, int y) a, (int x, int y) b) => (a.x - b.x, a.y - b.y);
        (int x, int y) Abs((int x, int y) a) => (Math.Abs(a.x), Math.Abs(a.y));

        private double Angle((int x, int y) start, (int x, int y) end)
        {
            return Math.Atan2(end.x - start.x, start.y - end.y); // transposed so it starts from up and goes clockwise, rather than right and anti
        }

        private int Distance((int x, int y) start, (int x, int y) end)
        {
            return (end.x - start.x) * (end.x - start.x) + (end.y - start.y) * (end.y - start.y);
        }

        public static int LCF(int a, int b)
        {
            while (a > 0)
            {
                int temp = b % a;
                b = a;
                a = temp;
            }

            return b;
        }

        private bool Obscures((int x, int y) source, (int x, int y) mid, (int x, int y) target)
        {
            var ms = Delta(mid, source);
            var tm = Delta(target, mid);

            if (ms.x != 0 && tm.x != 0 && Math.Sign(ms.x) != Math.Sign(tm.x)) return false;
            if (ms.y != 0 && tm.y != 0 && Math.Sign(ms.y) != Math.Sign(tm.y)) return false;

            if (ms.x == 0) return tm.x == 0 && Math.Abs(target.y - source.y) >= Math.Abs(ms.y);
            if (ms.y == 0) return tm.y == 0 && Math.Abs(target.x - source.x) >= Math.Abs(ms.x);


            return Math.Abs(tm.x) * Math.Abs(ms.y) == Math.Abs(tm.y) * Math.Abs(ms.x);
        }

        private int CountVisible((int, int) planet, List<(int, int)> planets)
        {
            var blocked = new List<(int, int)> { planet };

            foreach(var target in planets.Where(p => p != planet))
            {
                var obscured = planets.Where(p => p != planet && p != target).
                    Where(p => Obscures(planet, target, p));
                blocked.AddRange(obscured);
            }

            return planets.Count() - blocked.Distinct().Count();
        }
    }
}
