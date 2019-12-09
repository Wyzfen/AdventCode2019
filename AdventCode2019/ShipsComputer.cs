using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2019
{
    public class ShipsComputer
    {
        public enum Mode : int
        {
            Position,
            Immediate,
            Relative
        }

        public enum Instruction : int
        {
            Add = 1,
            Multiply = 2,
            Input = 3,
            Output = 4,
            JumpTrue = 5,
            JumpFalse = 6,
            LessThan = 7,
            Equals = 8,
            Rebase = 9,
            Exit = 99,
        }

        private int[] intcode;
        private int sp = 0; // Stack pointer
        private int rb = 0; // Relative base

        public ShipsComputer(int [] intcode)
        {
            this.intcode = (int []) intcode.Clone();
            //Array.Resize(ref this.intcode, this.intcode.Length + 1000); // TODO: make this dynamic!
        }

        public bool Completed { get; private set; }
        public int State => intcode[0];
        public int Noun { set => intcode[1] = value; }
        public int Verb { set => intcode[2] = value; }

        public static int Compute(int[] intcode, List<int> inputs = null) => Compute(null, null, intcode, inputs, out List<int> outputs);

        public static int Compute(int noun, int verb, int[] intcode, List<int> inputs = null) => Compute(noun, verb, intcode, inputs, out List<int> outputs);

        public static int Compute(int[] intcode, IList<int> inputs, out List<int> outputs) => Compute(null, null, intcode, inputs, out outputs);

        public static int Compute(int? noun, int? verb, int [] intcode, IList<int> inputs, out List<int> outputs)
        {
            ShipsComputer computer = new ShipsComputer(intcode);

            if (noun.HasValue) computer.Noun = noun.Value;
            if (verb.HasValue) computer.Verb = verb.Value;

            outputs = computer.Execute(inputs).ToList();

            return computer.State;
        }

        public IEnumerable<int> Execute(IEnumerable<int> inputs) => inputs != null ? inputs.SelectMany(i => Execute(i)) : Execute((int?) null);

        // Run with given inputs, return given outputs, until program stops, or required input missing
        public IEnumerable<int> Execute(int? input)
        {
            for (; sp < intcode.Length;)
            {
                Instruction instruction = (Instruction)(intcode[sp] % 100);
                int parameters = intcode[sp] / 100;
                
                int Inst(int o)
                {
                    if (parameters == 0) return intcode[intcode[sp + o]]; 

                    Mode param = (Mode)((parameters / (int)Math.Pow(10, o - 1)) % 10);
                    switch (param)
                    {
                        case Mode.Immediate:
                            return intcode[sp + o];
                        case Mode.Position:
                            return intcode[intcode[sp + o]];
                        case Mode.Relative:
                            return intcode[sp + o] + rb;
                    }

                    throw new IndexOutOfRangeException();
                }

                switch (instruction)
                {
                    case Instruction.Add:
                        intcode[intcode[sp + 3]] = Inst(1) + Inst(2);
                        sp += 4;
                        break;

                    case Instruction.Multiply:
                        intcode[intcode[sp + 3]] = Inst(1) * Inst(2);
                        sp += 4;
                        break;

                    case Instruction.Input:
                        if(! input.HasValue) yield break; // Stop executing if run out of inputs;

                        intcode[intcode[sp + 1]] = input.Value; // Must always be in position mode
                        input = null;

                        sp += 2;
                        break;

                    case Instruction.Output:
                        yield return Inst(1); // Must always be in position mode
                        sp += 2;
                        break;

                    case Instruction.JumpTrue:
                        sp = (Inst(1) != 0) ? Inst(2) : sp + 3;
                        break;

                    case Instruction.JumpFalse:
                        sp = (Inst(1) == 0) ? Inst(2) : sp + 3;
                        break;

                    case Instruction.LessThan:
                        intcode[intcode[sp + 3]] = (Inst(1) < Inst(2)) ? 1 : 0;
                        sp += 4;
                        break;

                    case Instruction.Equals:
                        intcode[intcode[sp + 3]] = (Inst(1) == Inst(2)) ? 1 : 0;
                        sp += 4;
                        break;

                    case Instruction.Rebase:
                        rb += Inst(1);
                        sp += 2;
                        break;

                    case Instruction.Exit: // exit
                        Completed = true;
                        yield break;

                    default: // error
                        throw new NotImplementedException();
                }
            }

            throw new StackOverflowException();
        }
    }
}
