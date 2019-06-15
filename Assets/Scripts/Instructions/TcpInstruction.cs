using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class TcpInstruction : Instruction {
        private RIArg firstArg;
        private RIArg secondArg;

        public TcpInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            if (args.Length != 2) {
                throw new InstructionValidationException("TCP must take two arguments");
            }
            firstArg = new RIArg(args[0], chip);
            secondArg = new RIArg(args[1], chip);
        }

        public override PCInstruction Execute() {
            var val1 = firstArg.GetValue(Chip);
            var val2 = secondArg.GetValue(Chip);
            if (val1 > val2)
                Chip.SetTestOutcome(TestPrefix.POSITIVE);
            else if (val1 < val2)
                Chip.SetTestOutcome(TestPrefix.NEGATIVE);
            else
                Chip.SetTestOutcome(TestPrefix.NONE);
            return PCInstruction.INCREMENT;
        }
    }
}