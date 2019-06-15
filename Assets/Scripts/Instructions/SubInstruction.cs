using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class SubInstruction : Instruction {
        private RIArg arg;

        public SubInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
           arg = new RIArg(args, chip); 
        }

        public override PCInstruction Execute() {
            var acc = Chip.ReadPort(Port.ACC);
            Chip.WritePort(Port.ACC, acc - arg.GetValue(Chip));
            return PCInstruction.INCREMENT;
        }
    }
}