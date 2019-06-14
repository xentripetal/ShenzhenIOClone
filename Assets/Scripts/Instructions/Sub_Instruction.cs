using Instructions.Models;

namespace Instructions {
    public class Sub_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "sub";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            var singleRiArg = new SingleRIArg(args, chip);
            return singleRiArg;
        }

        public void Execute(Chip chip, object argObj) {
            var singleRiArg = (SingleRIArg) argObj;
            chip.Accumulator -= singleRiArg.GetValue(chip);
        }
    }
}