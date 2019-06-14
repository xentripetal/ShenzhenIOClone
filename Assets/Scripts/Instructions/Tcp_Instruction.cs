using Instructions.Models;

namespace Instructions {
    public class Tcp_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "tcp";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.INCREMENT;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new DoubleRIRIArg(args, chip);
        }

        public void Execute(Chip chip, object argObj) {
            var ririArg = (DoubleRIRIArg) argObj;
            var val1 = ririArg.firstArg.GetValue(chip);
            var val2 = ririArg.secondArg.GetValue(chip);
            if (val1 > val2)
                chip.SetTestOutcome(TestPrefix.POSITIVE);
            else if (val1 < val2)
                chip.SetTestOutcome(TestPrefix.NEGATIVE);
            else
                chip.SetTestOutcome(TestPrefix.NONE);
        }
    }
}