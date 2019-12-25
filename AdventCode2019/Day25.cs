using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;

namespace AdventCode2019
{
    [TestClass]
    public class Day25
    {
        readonly long [] intCode = Utils.LongsFromCSVFile("day25.txt")[0];

        public static class Go
        {
            public static String North => "north\n";
            public static String South => "south\n";
            public static String East => "east\n";
            public static String West => "west\n";

            public static String Get(String item) => $"take {item}\n";
            public static String Put(String item) => $"drop {item}\n";

            public static String List => "inv\n";
        }

        [TestMethod]
        public void Problem1()
        {

            ShipsComputer robot = new ShipsComputer(intCode);
            string[] instructions = new string[] { null,
                    Go.West, 
                    Go.Get("semiconductor"), Go.West,
                    Go.Get("planetoid"), Go.West,
                    Go.Get("food ration"), Go.West,
                    Go.Get("fixed point"), Go.West,
                    Go.Get("klein bottle"), Go.East, Go.South, Go.West,
                    Go.Get("weather machine"), Go.East, Go.North, Go.East, Go.East, Go.South,
                    /*Go.Get("giant electromagnet"),*/ 
                    Go.South, Go.South, Go.Get("pointer"), 
                    Go.North, Go.North,
                    Go.East, Go.Get("coin"), Go.East,
                    /*Go.Get("photons"),*/ Go.North, Go.East, Go.List,
                    //// Go.South, Go.Get("molten lava"),
                    ////Go.South,
                    //////Go.West, Go.Get("infinite loop"), Go.East,
            };

            string[] lastScreen = null;
            foreach (var input in instructions)
            {
                long[] output = robot.Execute(input).ToArray();

                lastScreen = FormatOutput(output, false);
            }

            string[] items = lastScreen.Skip(2).Take(8).Select(i => i.Substring(2)).ToArray();
            int[] indices = Enumerable.Range(0, 8).ToArray();
            long result = 0;

            for (int i = 1; i < items.Length; i++)
            {
                foreach (var set in Utils.Combinations(indices, i))
                {
                    long[] output = null;

                    string[] drop = set.Select(index => items[index]).ToArray();
                    //Debug.WriteLine($"Dropping: {String.Join(", ", drop)}");

                    string[] dropCommands = drop.Select(d => Go.Put(d)).Append(Go.North).ToArray();
                    Array.ForEach(dropCommands, d => output = robot.Execute(d).ToArray());
                    string [] screen = FormatOutput(output, false);

                    if(!screen.Any(l => l.Contains("lighter") || l.Contains("heavier")))
                    {
                        Debug.WriteLine($"Passed, carrying: {String.Join(", ", indices.Except(set).Select(index => items[index]))}");

                        result = long.Parse(new String(screen.First(s => s.Contains("keypad")).Where(Char.IsDigit).ToArray()));

                        i = items.Length;
                        break;
                    }

                    string[] getCommands = drop.Select(d => Go.Get(d)).ToArray();
                    Array.ForEach(getCommands, g => output = robot.Execute(g).ToArray());
                    //FormatOutput(output);
                }
            }
            Assert.AreEqual(result, 34095120);
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

        public string [] FormatOutput(long [] output, bool print = true)
        {
            string image = new string(Array.ConvertAll(output, l => (char)l));
            string[] lines = image.Split(new char[] { (char)10 });

            if (print)
            {
                foreach (var line in lines)
                {
                    Debug.WriteLine(line);
                }

                Debug.WriteLine("");
            }
            return lines;
        }
    }
}
