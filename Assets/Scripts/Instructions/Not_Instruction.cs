using Instructions.Models;

namespace Instructions {
    public class Not_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "not";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new NoArg(args);
        }

        public void Execute(Chip chip, object argObj) {
            if (chip.Accumulator != 0)
                chip.Accumulator = 0;
            else
                chip.Accumulator = 100;
        }
    }
}