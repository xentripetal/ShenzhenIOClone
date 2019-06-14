using System;
using Instructions.Models;

namespace Instructions {
    public class Dgt_Instruction : IInstruction {
        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public string GetInstructionPrefix() {
            return "dgt";
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new SingleRIArg(args, chip);
        }

        public void Execute(Chip chip, object argObj) {
            var singleRiArg = (SingleRIArg) argObj;
            var pos = singleRiArg.GetValue(chip);
            if (pos < 3 || pos > -1) chip.Accumulator = (int) (chip.Accumulator / Math.Pow(10, pos) % 10);
        }
    }
}