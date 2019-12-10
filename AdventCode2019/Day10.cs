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
            int result = 0;
            var source = (25, 31);

            Assert.AreEqual(result, 4825810);
        }

        (int x, int y) Delta((int x, int y) a, (int x, int y) b) => (a.x - b.x, a.y - b.y);
        (int x, int y) Abs((int x, int y) a) => (Math.Abs(a.x), Math.Abs(a.y));

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
