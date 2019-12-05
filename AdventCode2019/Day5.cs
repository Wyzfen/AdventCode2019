using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day5
    {
        int[] intcode = Utils.StringsFromFile("day5.txt")[0].Select(s => int.Parse(s)).ToArray();

        [TestMethod]
        public void Problem1()
        {
            int result = ShipsComputer.Compute(intcode, new List<int> { 1 }, out List<int> outputs);

            Assert.IsTrue(result > 0);
            Assert.IsTrue(outputs.Take(outputs.Count - 1).All(o => o == 0));
            Assert.AreEqual(outputs.Last(), 15259545);
        }

        [TestMethod]
        public void Problem2()
        {
            List<int> outputs = null;
            int result = 0;

            #region Test cases
            /*
                        // Tests
                        int[] program = new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 };
                        result = ShipsComputer.Compute(program, new List<int> { 8 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 1);

                        result = ShipsComputer.Compute(program, new List<int> { 7 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 0);

                        program = new int[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 };
                        result = ShipsComputer.Compute(program, new List<int> { 7 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 1);

                        result = ShipsComputer.Compute(program, new List<int> { 8 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 0);

                        result = ShipsComputer.Compute(program, new List<int> { 9 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 0);

                        program = new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 };
                        result = ShipsComputer.Compute(program, new List<int> { 8 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 1);

                        result = ShipsComputer.Compute(program, new List<int> { 7 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 0);

                        program = new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 };
                        result = ShipsComputer.Compute(program, new List<int> { 7 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 1);

                        result = ShipsComputer.Compute(program, new List<int> { 8 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 0);

                        result = ShipsComputer.Compute(program, new List<int> { 9 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 0);

                        program = new int[] {3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                                             1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                                              999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 };
                        result = ShipsComputer.Compute(program, new List<int> { 7 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 999);

                        result = ShipsComputer.Compute(program, new List<int> { 8 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 1000);

                        result = ShipsComputer.Compute(program, new List<int> { 9 }, out outputs);
                        Assert.AreEqual(outputs.Count, 1);
                        Assert.AreEqual(outputs[0], 1001);
            */
            #endregion

            // Main program
            var inputs = new List<int> { 5 };
            result = ShipsComputer.Compute(intcode, inputs, out outputs);
            Assert.AreEqual(outputs.Count, 1);
            Assert.AreEqual(outputs[0], 7616021);
        }
    }
}
