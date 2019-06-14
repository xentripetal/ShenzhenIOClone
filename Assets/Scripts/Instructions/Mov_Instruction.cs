using Instructions.Models;

namespace Instructions {
    public class Mov_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "mov";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new DoubleRIRArg(args, chip);
        }

        public void Execute(Chip chip, object argObj) {
            var doubleRiArg = (DoubleRIRArg) argObj;
            chip.WritePort(doubleRiArg.secondArg.Port, doubleRiArg.firstArg.GetValue(chip));
        }
    }
}