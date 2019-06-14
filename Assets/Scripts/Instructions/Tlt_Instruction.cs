using Instructions.Models;

namespace Instructions {
    public class Tlt_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "tlt";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new DoubleRIRIArg(args, chip);
        }

        public void Execute(Chip chip, object argObj) {
            var ririArg = (DoubleRIRIArg) argObj;
            chip.SetTestOutcome(ririArg.firstArg.GetValue(chip) < ririArg.secondArg.GetValue(chip)
                ? TestPrefix.POSITIVE
                : TestPrefix.NEGATIVE);
        }
    }
}