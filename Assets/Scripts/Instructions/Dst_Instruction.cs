using System;
using Instructions.Models;

namespace Instructions {
    public class Dst_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "dst";
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new DoubleRIRIArg(args, chip);
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public void Execute(Chip chip, object argObj) {
            var doubleRiriArg = (DoubleRIRIArg) argObj;
            var pos = doubleRiriArg.firstArg.GetValue(chip);
            if (pos > 2 || pos < 0) return;


            pos = 2 - pos;
            var isNegative = chip.Accumulator < 0;
            var accStr = Math.Abs(chip.Accumulator).ToString();
            var newValue = (doubleRiriArg.secondArg.GetValue(chip) % 10).ToString();

            accStr = accStr.PadLeft(3, '0');

            var acc = int.Parse(accStr.Remove(pos, 1).Insert(pos, newValue));
            if (isNegative) acc *= -1;

            chip.Accumulator = acc;
        }
    }
}