using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day19
    {
        readonly long [] intCode = Utils.LongsFromCSVFile("day19.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            var grid = new Dictionary<(int x, int y), int>();

            long result = Enumerable.Range(0, 50).Select(x => Enumerable.Range(0, 50).Select(y =>
            {
                ShipsComputer.Compute(intCode, new List<long> { x, y }, out List<long> output);
                if (output.First() > 0) grid[(x, y)] = (int) output.First();

                return output.First();
            }).Count(i => i == 1)).Sum();

            Paint(grid);

            Assert.AreEqual(result, 131);
        }

        [TestMethod]
        public void Problem2()
        {
            var grid = new Dictionary<(int x, int y), int>();

            Enumerable.Range(100, 100).Select(x => Enumerable.Range(100, 100).Select(y =>
            {
                ShipsComputer.Compute(intCode, new List<long> { x, y }, out List<long> output);
                if (output.First() > 0) grid[(x, y)] = (int)output.First();

                return output.First();
            }).Count(i => i == 1)).Sum();

            //Paint(grid, 0, 0);

//            grid.Clear();

            long result = Enumerable.Range(100, 100).Select(k => 
            {
                int x = k;
                int y = (k * 665) / 1000;
                ShipsComputer.Compute(intCode, new List<long> {x, y}, out List<long> output);
                /*if (output.First() > 0)*/
                grid[(x, y)] = 2;// (int)output.First();

                return output.First();
            }).Sum();

            Paint(grid);

            // Upper = 0.63
            // Lower = 0.74

            Assert.AreEqual(result, 131);
        }

        string[] Paint(Dictionary<(int x, int y), int> input, int? minx = null, int? miny = null)
        {
            int maxX = input.Keys.Max(k => k.x);
            int maxY = input.Keys.Max(k => k.y);
            int minX = minx ?? input.Keys.Min(k => k.x);
            int minY = miny ?? input.Keys.Min(k => k.y);


            StringBuilder[] image = Enumerable.Range(0, maxY - minY + 1).Select(_ => new StringBuilder(new string('.', maxX - minX + 1))).ToArray();
            char[] pixels = { '·', '#', 'O' };

            foreach (var item in input.Where(i => i.Key.x >= minX && i.Key.y >= minY))
            {
                image[item.Key.y - minY][item.Key.x - minX] = pixels[item.Value];
            }

            foreach (var line in image)
            {
                System.Diagnostics.Debug.WriteLine(line);
            }

            return image.Select(i => i.ToString()).ToArray();
        }
    }
}
