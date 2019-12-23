using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;

namespace AdventCode2019
{
    [TestClass]
    public class Day23
    {
        readonly long [] intCode = Utils.LongsFromCSVFile("day23.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            ShipsComputer [] nics = Enumerable.Range(0, 50).Select(r =>
            {
                var c = new ShipsComputer(intCode);
                c.Execute(r).ToArray(); // execute.toarray forces completion of execute command
                return c;
            }).ToArray();

            List<long>[] EmptyInputs() => Enumerable.Range(0, 50).Select(_ => new List<long> { -1L }).ToArray();

            IEnumerable<long>[] outputs = null;
            List<long>[] inputs = EmptyInputs();
            long result = 0;

            do
            {
                outputs = nics.Zip(inputs, (nic, inp) => nic.Execute(inp).ToArray()).ToArray(); // execute.toarray forces completion of execute command
                inputs = EmptyInputs();
                foreach(var output in outputs.Select(o => new Queue<long>(o)))
                {
                    while(output.Any())
                    {
                        // Check for multiple outputs per NIC
                        (long dest, long x, long y) = (output.Dequeue(), output.Dequeue(), output.Dequeue());

                        if (dest == 255)
                        {
                            result = y;
                            break;
                        }

                        // Append to allow multiple inputs per NIC
                        var input = inputs[dest];
                        if (input.Count() == 1)
                        {
                            inputs[dest].Clear();
                        }

                        inputs[dest].Add(x);
                        inputs[dest].Add(y);
                    }
                }
            } while (result == 0);

            Assert.AreEqual(result, 16549);
        }

        [TestMethod]
        public void Problem2()
        {
            ShipsComputer[] nics = Enumerable.Range(0, 50).Select(r =>
            {
                var c = new ShipsComputer(intCode);
                c.Execute(r).ToArray(); // execute.toarray forces completion of execute command
                return c;
            }).ToArray();

            List<long>[] EmptyInputs() => Enumerable.Range(0, 50).Select(_ => new List<long> { -1L }).ToArray();

            IEnumerable<long>[] outputs = null;
            List<long>[] inputs = EmptyInputs();
            (long x, long y) nat = (-1, -1), previous = (-1, -1);

            long result = 0;

            do
            {
                outputs = nics.Zip(inputs, (nic, inp) => nic.Execute(inp).ToArray()).ToArray(); // execute.toarray forces completion of execute command
                inputs = EmptyInputs();
                foreach (var output in outputs.Select(o => new Queue<long>(o)))
                {
                    while (output.Any())
                    {
                        // Check for multiple outputs per NIC
                        (long dest, long x, long y) = (output.Dequeue(), output.Dequeue(), output.Dequeue());

                        if (dest == 255)
                        {
                            nat = (x, y);
                            continue;
                        }

                        // Append to allow multiple inputs per NIC
                        var input = inputs[dest];
                        if (input.Count() == 1)
                        {
                            inputs[dest].Clear();
                        }

                        inputs[dest].Add(x);
                        inputs[dest].Add(y);
                    }
                }

                if(inputs.All(i => i.Count() == 1))
                {
                    if(previous == nat)
                    {
                        result = nat.y;
                    }
                    previous = nat;

                    Debug.WriteLine($"IDLE. NAT = {nat.x}, {nat.y}");
                    inputs[0].Clear();
                    inputs[0].Add(nat.x);
                    inputs[0].Add(nat.y);
                }
            } while (result == 0);

            Assert.AreEqual(result, 11462);
        }
    }
}
