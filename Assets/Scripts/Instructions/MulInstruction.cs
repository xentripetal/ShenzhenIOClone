using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class MulInstruction : Instruction {
        private RIArg arg;
        
        public MulInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            arg = new RIArg(args, chip);
        }

        public override PCInstruction Execute() {
            var acc = Chip.ReadPort(Register.ACC);
            Chip.WritePort(Register.ACC, acc * arg.GetValue(Chip));
            return PCInstruction.INCREMENT;
        }
    }
}