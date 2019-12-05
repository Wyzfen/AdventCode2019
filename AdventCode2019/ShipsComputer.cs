using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019
{
    static class ShipsComputer
    {
        public enum Mode : int
        {
            Position,
            Immediate,
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
            Exit = 99,
        }

        public static int Compute(int[] intcode, List<int> inputs = null) => Compute(null, null, intcode, inputs, out List<int> outputs);

        public static int Compute(int noun, int verb, int[] intcode, List<int> inputs = null) => Compute(noun, verb, intcode, inputs, out List<int> outputs);

        public static int Compute(int[] intcode, IList<int> inputs, out List<int> outputs) => Compute(null, null, intcode, inputs, out outputs);

        public static int Compute(int? noun, int? verb, int [] intcode, IList<int> inputs, out List<int> outputs)
        {
            intcode = (int [])intcode.Clone(); // Make it so that the program passed in isnt modified
            outputs = new List<int>();

            intcode[1] = noun ?? intcode[1];
            intcode[2] = verb ?? intcode[2];

            for (int sp = 0; sp < intcode.Length;)
            {
                Instruction instruction = (Instruction) (intcode[sp] % 100);
                int parameters = intcode[sp] / 100;

                int Inst(int o) => (parameters == 0 || (Mode)((parameters / (int)Math.Pow(10, o - 1)) % 10) == Mode.Position) ? intcode[intcode[sp + o]] : intcode[sp + o];

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
                        intcode[intcode[sp + 1]] = inputs[0]; // Must always be in position mode
                        inputs.RemoveAt(0);
                        sp += 2;
                        break;

                    case Instruction.Output:
                        outputs.Add(Inst(1)); // Must always be in position mode
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

                    case Instruction.Exit: // exit
                        return intcode[0];

                    default: // error
                        return -1;
                }
            }

            return -1;
        }
    }
}
