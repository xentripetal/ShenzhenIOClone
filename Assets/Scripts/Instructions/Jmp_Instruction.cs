using Instructions.Models;

namespace Instructions {
    public class Jmp_Instruction : IInstruction {
        public string GetInstructionPrefix() {
            return "jmp";
        }

        public PCInstruction GetPCInstruction() {
            return PCInstruction.NOTHING;
        }

        public object GenerateArgObj(Chip chip, string[] args) {
            return new LabelArg(args, chip);
        }

        public void Execute(Chip chip, object argObj) {
            var labelArg = (LabelArg) argObj;
            chip.GoToLabel(labelArg.Label);
        }
    }
}