using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day13
    {
        readonly long [] intCode = Utils.LongsFromCSVFile("day13.txt")[0];

        public enum Tile : int
        {
            Empty = 0,
            Wall = 1,
            Block = 2,
            Paddle = 3,
            Ball = 4
        }

        [TestMethod]
        public void Problem1()
        {
            ShipsComputer arcadeMachine = new ShipsComputer(intCode);

            var screen = new Dictionary<(int x, int y), Tile>();
            ArcadeMachine(arcadeMachine, null, screen);

            long result = screen.Values.Count(v => v == Tile.Block);

            Assert.AreEqual(result, 372);

            #region Display
#if DEBUG
            foreach (var line in ArcadeDisplay(screen))
            {
                System.Diagnostics.Debug.WriteLine(line);
            }

            System.Diagnostics.Debug.WriteLine($"Score: { (screen.TryGetValue((-1, 0), out Tile s) ? (int)s : 0) }");
#endif
            #endregion
        }

        [TestMethod]
        public void Problem2()
        {
            intCode[0] = 2; // put coin in machine

            ShipsComputer arcadeMachine = new ShipsComputer(intCode);

            var screen = new Dictionary<(int x, int y), Tile>();

            int blockCount = 0;
            int score = 0;
            (int x, int y) ball;
            (int x, int y) paddle;
            int input = 0;

            do
            {
                ArcadeMachine(arcadeMachine, input, screen);
                
                ball = screen.FirstOrDefault(x => x.Value == Tile.Ball).Key;
                paddle = screen.FirstOrDefault(x => x.Value == Tile.Paddle).Key;
                score = screen.TryGetValue((-1, 0), out Tile s) ? (int) s : score;

                blockCount = screen.Values.Count(v => v == Tile.Block);

                input = ball.x.CompareTo(paddle.x); // try to keep paddle under ball. only needs to be within 1 to hit.

                #region Display
#if DEBUG
                foreach (var line in ArcadeDisplay(screen))
                {
                    System.Diagnostics.Debug.WriteLine(line);
                }

                System.Diagnostics.Debug.WriteLine($"Score: {score}, Blocks: {blockCount}, Ball: {ball}, Paddle: {paddle}\n");
#endif
                #endregion
            } while (blockCount > 0 && ball.y < paddle.y );

            Assert.AreEqual(score, 19297);
        }

        public void ArcadeMachine(ShipsComputer arcadeMachine, int? input, Dictionary<(int x, int y), Tile> screen)
        {
            var output = arcadeMachine.Execute(input).ToArray();

            for (int i = 0; i < output.Length; i += 3)
            {
                int x = (int)output[i];
                int y = (int)output[i + 1];
                Tile id = (Tile)output[i + 2];

                screen[(x, y)] = id;
            }
        }

        public string [] ArcadeDisplay(Dictionary<(int x, int y), Tile> screen)
        {
            int maxY = screen.Keys.Max(k => k.y);
            int maxX = screen.Keys.Max(k => k.x);

            StringBuilder[] image = Enumerable.Range(0, maxY + 1).Select(_ => new StringBuilder(new string(' ', maxX + 1))).ToArray();

            char[] pixels = { ' ', '▓', '▧', '▤', '○' };

            foreach (var item in screen)
            {
                if (item.Key.x > -1)
                {
                    image[item.Key.y][item.Key.x] = pixels[(int)item.Value];
                }
            }

            // for fun, draw wider paddle
            var paddle = screen.FirstOrDefault(x => x.Value == Tile.Paddle).Key;
            image[paddle.y][paddle.x - 1] = '◁';
            image[paddle.y][paddle.x + 1] = '▷';


            return image.Select(i => i.ToString()).ToArray();
        }
    }
}
