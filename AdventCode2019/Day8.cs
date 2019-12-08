using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day8
    {
        readonly string input = File.ReadAllLines("day8.txt").First();
        readonly int X = 25;
        readonly int Y = 6;

        [TestMethod]
        public void Problem1()
        {
            int chunkSize = X * Y;
            var chunks = Enumerable.Range(0, input.Length / chunkSize)
                                   .Select(i => input.Substring(i * chunkSize, chunkSize)).ToList();
            var counts = chunks.Select(c => c.Count(i => i == '0')).ToList();
            var select = counts.IndexOf(counts.Min());

            int result = chunks[select].Count(i => i == '1') * chunks[select].Count(i => i == '2');

            Assert.AreEqual(result, 1806);
        }

        [TestMethod]
        public void Problem2()
        {
            int chunkSize = X * Y;
            var chunks = Enumerable.Range(0, input.Length / chunkSize)
                                   .Select(i => input.Substring(i * chunkSize, chunkSize)).ToList();

            StringBuilder[] image = Enumerable.Range(0, Y).Select(_ => new StringBuilder(new string(' ', X))).ToArray();
            char[] pixels = { '·' , '█', ' '};

            for(int x = 0; x < X; x ++)
            {
                for(int y = 0; y < Y; y ++)
                {
                    image[y][x] = pixels[chunks.Select(c => c[x + y * X] - '0').First(c => c != 2)];
                }
            }

            foreach(var line in image)
            {
                System.Diagnostics.Debug.WriteLine(line);
            }

            Assert.AreEqual(image[0].ToString(), "··██··██··████·███···██··");
            Assert.AreEqual(image[1].ToString(), "···█·█··█·█····█··█·█··█·");
            Assert.AreEqual(image[2].ToString(), "···█·█··█·███··█··█·█··█·");
            Assert.AreEqual(image[3].ToString(), "···█·████·█····███··████·");
            Assert.AreEqual(image[4].ToString(), "█··█·█··█·█····█·█··█··█·");
            Assert.AreEqual(image[5].ToString(), "·██··█··█·█····█··█·█··█·");
        }
    }
}
