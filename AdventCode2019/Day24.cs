using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day24
    {
        string[] input = new string[]
        {
           "..#.#",
           "#####",
           ".#...",
           "...#.",
           "##...",
        };

        [TestMethod]
        public void Problem1()
        {
            //string[] test = new string[]
            //{
            //   "....#",
            //   "#..#.",
            //   "#..##",
            //   "..#..",
            //   "#....",
            //};

            // Initialise with start level and 2 empty levels as they're adjacent
            var test = new Dictionary<int, string[]>
            {
                [0] = (string[])input.Clone(),
                [1] = Enumerable.Repeat(".....", 5).ToArray(),
                [-1] = Enumerable.Repeat(".....", 5).ToArray()
            };

            List<int> bioDiversities = new List<int> { BioDiversity(test[0]) };

            int result = 0;

            do
            {
                test[0] = Bugs(test);
                int bioRating = BioDiversity(test[0]);
                if (bioDiversities.Contains(bioRating))
                {
                    result = bioRating;
                    break;
                }
                bioDiversities.Add(bioRating);
            } while (true);

            Assert.AreEqual(result, 18407158);
        }

        [TestMethod]
        public void Problem2()
        {
            var test = new Dictionary<int, string[]>
            {
                [0]  = (string[])input.Clone(), /*
                [0]  = new string[]
                        {
                           "....#",
                           "#..#.",
                           "#.?##",
                           "..#..",
                           "#...."
                        }, //*/
                [-1] = Enumerable.Repeat(".....", 5).ToArray(),
                [1]  = Enumerable.Repeat(".....", 5).ToArray()
            };


            for (int i = 0; i < 200; i++)
            {
                var output = new Dictionary<int, string[]>();
                foreach (var index in test.Keys.ToArray())
                {
                    output[index] = Bugs(test, index, true);
                }
                
                foreach(var pair in output)
                {
                    test[pair.Key] = pair.Value;
                }
            }

            int result = test.Select(t => t.Value.Sum(r => r.Count(c => c == '#'))).Sum();
            
            Assert.AreEqual(result, 1998);
        }

        private string [] Bugs(Dictionary<int,string []> inputs, int level = 0, bool warp = false)
        {
            string[] input = inputs[level];
            StringBuilder [] output = new StringBuilder[5];

            for (int y = 0; y < 5; y++)
            {
                output[y] = new StringBuilder(input[y]);

                for (int x = 0; x < 5; x++)
                {
                    if (warp && x == 2 && y == 2) continue; // don't check the centre

                    int bugs = 0;

                    // Check for warps
                    if (warp)
                    {
                        if ((y == 2 && (x == 1 || x == 3))
                            || (x == 2 && (y == 1 || y == 3)))
                        {
                            bugs += BugsInner(inputs, level, x, y);
                        }
                        else if (x == 0 || x == 4 || y == 0 || y == 4)
                        {
                            bugs += BugsOuter(inputs, level, x, y);
                        }
                    }

                    if (x > 0 && !(warp && x == 3 && y == 2) && input[y][x - 1] == '#') bugs++;
                    if (x < 4 && !(warp && x == 1 && y == 2) && input[y][x + 1] == '#') bugs++;
                    if (y > 0 && !(warp && x == 2 && y == 3) && input[y - 1][x] == '#') bugs++;
                    if (y < 4 && !(warp && x == 2 && y == 1) && input[y + 1][x] == '#') bugs++;

                    if (input[y][x] == '#' && bugs != 1) output[y][x] = '.';
                    if (input[y][x] == '.' && (bugs == 1 || bugs == 2)) output[y][x] = '#';
                }
            }

            return output.Select(o => o.ToString()).ToArray();
        }

        private int BugsInner(Dictionary<int, string[]> inputs, int level, int x, int y)
        {
            // Create level if it doesn't exist
            if(!inputs.TryGetValue(level - 1, out string [] inner))
            {
                if (inputs[level].Any(r => r.Any(c => c == '#'))) // only create a new level if this one isn't empty
                {
                    inputs[level - 1] = Enumerable.Repeat(".....", 5).ToArray();
                }
                return 0;
            }

            int bugs = 0;
            if (x == 1 && y == 2) bugs += inner.Count(r => r[0] == '#');
            if (x == 3 && y == 2) bugs += inner.Count(r => r[4] == '#');
            if (x == 2 && y == 1) bugs += inner[0].Count(r => r == '#');
            if (x == 2 && y == 3) bugs += inner[4].Count(r => r == '#');

            return bugs;
        }

        private int BugsOuter(Dictionary<int, string[]> inputs, int level, int x, int y)
        {
            // Create level if it doesn't exist
            if (!inputs.TryGetValue(level + 1, out string[] outer))
            {
                if (inputs[level].Any(r => r.Any(c => c == '#'))) // only create a new level if this one isn't empty
                {
                    inputs[level + 1] = Enumerable.Repeat(".....", 5).ToArray();
                }
                return 0;
            }

            int bugs = 0;
            if (x == 0) bugs += outer[2][1] == '#' ? 1 : 0;
            if (x == 4) bugs += outer[2][3] == '#' ? 1 : 0;
            if (y == 0) bugs += outer[1][2] == '#' ? 1 : 0;
            if (y == 4) bugs += outer[3][2] == '#' ? 1 : 0;

            return bugs;
        }

        private int BioDiversity(string [] input)
        {
            int result = 0;
            for(int y = 0; y < 5; y++)
            {
                string row = input[y];
                for (int x = 0; x < 5; x++)
                {
                    char cell = row[x];
                    if (cell == '#')
                    {
                        result += (1 << (y * 5 + x));
                    }
                }
            }

            return result;
        }
    }
}
