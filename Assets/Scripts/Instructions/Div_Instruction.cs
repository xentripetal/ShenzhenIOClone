using Instructions.Models;

namespace Instructions {
    public class Div_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "div";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new SingleRIArg(args, chip);
        }

        public void Execute(Chip chip, object argObj) {
            var singleRiArg = (SingleRIArg) argObj;
            chip.Accumulator /= singleRiArg.GetValue(chip);
        }
    }
}