using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class MovInstruction : Instruction {
        private RIArg src;
        private RArg dst;

        private int cachedVal = -1000;
        
        public MovInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            if (args.Length != 2) {
                throw new InstructionValidationException("mov must take 2 args");
            }
            src = new RIArg(args[0], chip);
            dst = new RArg(args[1], chip);
        }

        public override PCInstruction Execute() {
            if (cachedVal == -1000) {
                var val = src.GetValue(Chip);

                if (val == -1000) {
                    return PCInstruction.NOTHING;
                }
                cachedVal = val;
            }


            var writeResponse = Chip.WritePort(dst.Register, cachedVal);
            if (writeResponse == WritePortResponse.WAITING || writeResponse == WritePortResponse.REGISTERED) {
                return PCInstruction.NOTHING;
            }

            cachedVal = -1000;
            return PCInstruction.INCREMENT;
        }
    }
}