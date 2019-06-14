using Instructions.Models;

namespace Instructions {
    public class Nop_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "nop";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new NoArg(args);
        }

        public void Execute(Chip chip, object argObj) {
        }
    }
}