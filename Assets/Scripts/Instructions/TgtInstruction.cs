using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class TgtInstruction : Instruction {
        private RIArg firstArg;
        private RIArg secondArg;

        public TgtInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            if (args.Length != 2) {
                throw new InstructionValidationException("Must take two arguments");
            }
            firstArg = new RIArg(args[0], chip);
            secondArg = new RIArg(args[1], chip);
        }

        public override PCInstruction Execute() {
            var val1 = firstArg.GetValue(Chip);
            var val2 = secondArg.GetValue(Chip);
            Chip.SetTestOutcome(val1 > val2 ? TestPrefix.POSITIVE : TestPrefix.NEGATIVE);
            return PCInstruction.INCREMENT;
        }
    }
}