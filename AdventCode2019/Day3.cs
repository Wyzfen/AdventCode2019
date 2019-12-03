using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day3
    {
        static readonly string[][] input = Utils.StringsFromFile("day3.txt");

        [TestMethod]
        public void Problem1()
        {

            var first = FollowWire(input[0]);
            var second = FollowWire(input[1]);

            var intersections = first.Intersect(second);
            var result = intersections.Select((v) => v.Item1 + v.Item2).Min();

            Assert.AreEqual(result, 3247);
        }

        [TestMethod]
        public void Problem2()
        {
            //var input = Utils.StringsFromString("R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62, R66, U55, R34, D71, R55, D58, R83");
            //var input2 = Utils.StringsFromString("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\n U98, R91, D20, R16, D67, R40, U7, R15, U6, R7");
            var first = FollowWireDist(input[0]);
            var second = FollowWireDist(input[1]);

            var intersections = first.Keys.Intersect(second.Keys);

            var result = intersections.Select(l => first[l] + second[l]).Min();

            Assert.AreEqual(result, 48054);
        }

        private (int x, int y) GetMovement(string move)
        {
            char instruction = move[0];
            int distance = int.Parse(move.Substring(1));

            switch(instruction)
            {
                case 'U': return (0, -distance);
                case 'D': return (0, distance);
                case 'L': return (-distance, 0);
                case 'R': return (distance, 0);
            }

            throw new FormatException();
        }

        private IEnumerable<(int, int)> Move(int startx, int starty, int deltax, int deltay)
        {
            if (deltax != 0)
            {
                var xrange = Enumerable.Range(Math.Min(startx + deltax + 1, startx), Math.Abs(deltax)).Select(x => (x, starty));
                if (deltax < 0) xrange = xrange.Reverse();
                return xrange;
            }
            else
            {
                var yrange = Enumerable.Range(Math.Min(starty + deltay + 1, starty), Math.Abs(deltay)).Select(y => (startx, y));
                if (deltay < 0) yrange = yrange.Reverse();
                return yrange;
            }
        }

        private IEnumerable<(int, int)> FollowWire(string [] wire)
        {
            List<(int, int)> result = new List<(int, int)>();
            int x = 0, y = 0;

            foreach (string value in wire)
            {
                (var deltax, var deltay) = GetMovement(value.Trim());
                var locations = Move(x, y, deltax, deltay);
                result.AddRange(locations);

                x += deltax;
                y += deltay;
            }

            result.RemoveAll(v => v == (0, 0));

            return result;
        }

        private Dictionary<(int, int), int> FollowWireDist(string[] wire)
        {
            var path = FollowWire(wire);

            //return path.Select((v, r) => (v, r + 1)).GroupBy(k => k.v).ToDictionary(g => g.Key, g => g.Min(e => e.Item2//));

            var results = new Dictionary<(int, int), int>();
            
            int count = 1; // no start point
            foreach(var step in path)
            {
                if(!results.ContainsKey(step))
                {
                    results.Add(step, count);
                }

                count++;
            }

            return results;
        }
    }
}
