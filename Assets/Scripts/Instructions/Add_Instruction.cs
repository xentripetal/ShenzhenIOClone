using Instructions.Models;

namespace Instructions {
    public class Add_Instruction : IInstruction {
        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public string GetInstructionPrefix() {
            return "add";
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new SingleRIArg(args, chip);
        }

        public void Execute(Chip chip, object argObj) {
            var singleRiArg = (SingleRIArg) argObj;
            chip.Accumulator += singleRiArg.GetValue(chip);
        }
    }
}