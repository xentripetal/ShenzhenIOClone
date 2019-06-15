using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class NopInstruction : Instruction {
        public NopInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            if (args.Length != 0) {
                throw new InstructionValidationException("nop takes no args");
            }
        }

        public override PCInstruction Execute() {
            return PCInstruction.INCREMENT;
        }
    }
}