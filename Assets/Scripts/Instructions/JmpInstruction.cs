using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class JmpInstruction : Instruction {
        private string label;
        public JmpInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            if (args.Length != 1) throw new InstructionValidationException("Must take a single label arg");

            if (!chip.HasLabel(args[0]))
                throw new InstructionValidationException($"The label {args[0]} does not exist.");

            label = args[0];
        }

        public override PCInstruction Execute() {
            Chip.GoToLabel(label);
            return PCInstruction.NOTHING;
        }
    }
}