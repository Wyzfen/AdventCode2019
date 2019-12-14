using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day14
    {
        //26 DTQTB, 16 TWHBV => 3 JMGDP

        static IEnumerable<string> input = Utils.StringsFromFile("day14.txt");

        List<Reaction> reactions = input.Select(i => new Reaction(i)).ToList();

        private class Reaction
        {
            public (string name, ulong amount) Output;
            public List<(string name, ulong amount)> Inputs;

            private (string name, ulong amount) SplitReagent(string input)
            {
                (var a, var n, _) = input.Trim().Split(' ');
                return (n.Trim(), ulong.Parse(a));
            }

            public Reaction(string reactionString)
            {
                (string input, string output, _) = reactionString.Split(new string[] { "=>" }, StringSplitOptions.None);

                Inputs = input.Split(',').Select(i => SplitReagent(i)).ToList();
                Output = SplitReagent(output);
            }
        }

        [TestMethod]
        public void Problem1()
        {
            //var test = new string[] { "10 ORE => 10 A",
            //                        "1 ORE => 1 B",
            //                        "7 A, 1 B => 1 C",
            //                        "7 A, 1 C => 1 D",
            //                        "7 A, 1 D => 1 E",
            //                        "7 A, 1 E => 1 FUEL"};

            //var test = new string[] { "9 ORE => 2 A",
            //                        "8 ORE => 3 B",
            //                        "7 ORE => 5 C",
            //                        "3 A, 4 B => 1 AB",
            //                        "5 B, 7 C => 1 BC",
            //                        "4 C, 1 A => 1 CA",
            //                        "2 AB, 3 BC, 4 CA => 1 FUEL" };

           //var test = new string[] { "171 ORE => 8 CNZTR",
           //                         "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
           //                         "114 ORE => 4 BHXH",
           //                         "14 VRPVC => 6 BMBT",
           //                         "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
           //                         "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
           //                         "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
           //                         "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
           //                         "5 BMBT => 4 WPTQ",
           //                         "189 ORE => 9 KTJDG",
           //                         "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
           //                         "12 VRPVC, 27 CNZTR => 2 XDBXC",
           //                         "15 KTJDG, 12 BHXH => 5 XCVML",
           //                         "3 BHXH, 2 VRPVC => 7 MZWV",
           //                         "121 ORE => 7 VRPVC",
           //                         "7 XCVML => 6 RJRHP",
           //                         "5 BHXH, 4 VRPVC => 5 LTCX"};

            //var reactions = test.Select(s => new Reaction(s));

            var materials = new Dictionary<string, ulong>();

            ulong result = Process("FUEL", 1, materials, reactions);

            Assert.AreEqual(result, (ulong) 1065255);
        }

        [TestMethod]
        public void Problem2()
        {
            long target = 1000000000000;

            long high = 2 * target / 1065255; // start based on solution for 1.
            long low = 0;

            long current = 0;

            do
            {
                current = (high + low) / 2;

                var materials = new Dictionary<string, ulong>();
                long output = (long)Process("FUEL", (ulong)current, materials, reactions);

                if (output < target)
                {
                    low = current;
                }
                else if (output > target)
                {
                    high = current;
                }

                System.Diagnostics.Debug.WriteLine($"{current} -> {output} ... {low}<->{high} -> {(low + high) / 2}");
            } while (high > low && current != (high + low) / 2);


            Assert.AreEqual(low, 1766154);
        }


        private ulong Process(string requiredMaterial, ulong requiredAmount, Dictionary<string, ulong> materials, IEnumerable<Reaction> reactions)
        {
            if (requiredMaterial == "ORE")
            {
                return requiredAmount;
            }

            var reaction = reactions.First(r => r.Output.name == requiredMaterial);
            materials.TryGetValue(requiredMaterial, out ulong have);

            ulong ore = 0;

            if (requiredAmount > have)
            {
                ulong multiple = Count(requiredAmount - have, reaction.Output.amount);

                foreach (var (name, amount) in reaction.Inputs)
                {
                    ore += Process(name, amount * multiple, materials, reactions);
                }

                have += multiple * reaction.Output.amount;
            }

            materials[requiredMaterial] = have - requiredAmount;
            
            return ore;
        }

        public ulong Count(ulong value, ulong den) => (value + den - 1) / den;
    }
}
