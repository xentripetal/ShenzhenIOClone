using Instructions.Models;

namespace Instructions {
    public class Swp_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "swp";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new NoArg(args);
        }

        public void Execute(Chip chip, object argObj) {
            var acc = chip.Accumulator;
            chip.Accumulator = chip.DataRegister;
            chip.DataRegister = acc;
        }
    }
}