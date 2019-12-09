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

        private long [] intcode;
        private int sp = 0; // Stack pointer
        private int rb = 0; // Relative base

        public ShipsComputer(long [] intcode)
        {
            this.intcode = (long []) intcode.Clone();
            Array.Resize(ref this.intcode, this.intcode.Length + 10000); // TODO: make this dynamic!
        }

        public bool Completed { get; private set; }
        public long State => intcode[0];
        public long Noun { set => intcode[1] = value; }
        public long Verb { set => intcode[2] = value; }

        public static long Compute(long [] intcode, List<long> inputs = null) => Compute(null, null, intcode, inputs, out List<long> outputs);

        public static long Compute(long noun, long verb, long [] intcode, List<long> inputs = null) => Compute(noun, verb, intcode, inputs, out List<long> outputs);

        public static long Compute(long [] intcode, IList<long> inputs, out List<long> outputs) => Compute(null, null, intcode, inputs, out outputs);

        public static long Compute(long? noun, long? verb, long [] intcode, IList<long> inputs, out List<long> outputs)
        {
            ShipsComputer computer = new ShipsComputer(intcode);

            if (noun.HasValue) computer.Noun = noun.Value;
            if (verb.HasValue) computer.Verb = verb.Value;

            outputs = computer.Execute(inputs).ToList();

            return computer.State;
        }

        public IEnumerable<long> Execute(IEnumerable<long> inputs) => inputs != null ? inputs.SelectMany(i => Execute(i)) : Execute((long?) null);

        // Run with given inputs, return given outputs, until program stops, or required input missing
        public IEnumerable<long> Execute(long? input)
        {
            for (; sp < intcode.Length;)
            {
                Instruction instruction = (Instruction)(intcode[sp] % 100);
                long parameters = intcode[sp] / 100;

                long Inst(int o) => intcode[Index(o)];

                int Index(int o)
                {
                    Mode param = parameters == 0 ? Mode.Position : (Mode)((parameters / (int)Math.Pow(10, o - 1)) % 10);

                    switch (param)
                    {
                        case Mode.Position:
                            return (int) intcode[sp + o];
                        case Mode.Immediate:
                            return sp + o;
                        case Mode.Relative:
                            return (int)(intcode[sp + o]) + rb;
                    }

                    // Tried to resize array here based on index, but doesn't work when call inside set indexer. e.g. intcode[Index(3)] = ...

                    throw new ArgumentOutOfRangeException();
                }

                switch (instruction)
                {
                    case Instruction.Add:
                        intcode[Index(3)] = Inst(1) + Inst(2);
                        sp += 4;
                        break;

                    case Instruction.Multiply:
                        intcode[Index(3)] = Inst(1) * Inst(2);
                        sp += 4;
                        break;

                    case Instruction.Input:
                        if(! input.HasValue) yield break; // Stop executing if run out of inputs;

                        intcode[Index(1)] = input.Value; // Must never be in immediate mode
                        input = null;

                        sp += 2;
                        break;

                    case Instruction.Output:
                        yield return Inst(1); // Must never be in immediate mode
                        sp += 2;
                        break;

                    case Instruction.JumpTrue:
                        sp = (Inst(1) != 0) ? (int) Inst(2) : sp + 3;
                        break;

                    case Instruction.JumpFalse:
                        sp = (Inst(1) == 0) ? (int) Inst(2) : sp + 3;
                        break;

                    case Instruction.LessThan:
                        intcode[Index(3)] = (Inst(1) < Inst(2)) ? 1 : 0;
                        sp += 4;
                        break;

                    case Instruction.Equals:
                        intcode[Index(3)] = (Inst(1) == Inst(2)) ? 1 : 0;
                        sp += 4;
                        break;

                    case Instruction.Rebase:
                        rb += (int) Inst(1);
                        sp += 2;
                        break;

                    case Instruction.Exit: // exit
                        Completed = true;
                        yield break;

                    default: // error
                        throw new InvalidOperationException();
                }
            }

            throw new StackOverflowException();
        }
    }
}
