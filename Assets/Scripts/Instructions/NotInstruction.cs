using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class NotInstruction : Instruction {
        public NotInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            if (args.Length != 0) {
                throw new InstructionValidationException("not takes no args");
            }
        }

        public override PCInstruction Execute() {
            var acc = Chip.ReadPort(Register.ACC);
            Chip.WritePort(Register.ACC, acc != 0 ? 0 : 100);
            return PCInstruction.INCREMENT;
        }
    }
}