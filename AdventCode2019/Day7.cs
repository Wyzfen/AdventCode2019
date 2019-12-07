using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day7
    {
        readonly int [] intCode = Utils.IntsFromCSVFile("day7.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            int Amps(List<int> phases)
            {
                int output = 0;

                foreach (int phase in phases)
                {
                    int computed = ShipsComputer.Compute(intCode, new List<int> { phase, output }, out var outputs);
                    output = outputs.Last();
                }

                return output;
            }

            var phasePermutations = Utils.Permutations( new int [] { 0, 1, 2, 3, 4 } );

            int result = phasePermutations.Max(p => Amps(p));

            Assert.AreEqual(result, 338603);
        }

        [TestMethod]
        public void Problem2()
        {
            int Amps(List<int> phases)
            {
                ShipsComputer[] amps = new ShipsComputer[5].Select(_ => new ShipsComputer(intCode)).ToArray();

                int output = 0;
                while(true)
                {
                    for (int ampIndex = 0; ampIndex < 5; ampIndex++)
                    {
                        var inputs = new List<int> { output };
                        if (phases != null) inputs.Insert(0, phases[ampIndex]);

                        var outputs = amps[ampIndex].Execute(inputs).ToArray();
                        if (outputs.Count() == 0)
                        {
                            return output;
                        }

                        output = outputs.Last();
                    }

                    phases = null;
                }
            }

            var phasePermutations = Utils.Permutations(new int[] { 5, 6, 7, 8, 9 });

            int result = phasePermutations.Max(p => Amps(p));

            Assert.AreEqual(result, 63103596);
        }
    }
}
