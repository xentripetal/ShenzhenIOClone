using Zachclone;
using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class AddInstruction : Instruction {
        private RIArg arg;
        
        public AddInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            arg = new RIArg(args, chip);
        }

        public override PCInstruction Execute() {
            Chip.WritePort(Port.ACC, Chip.ReadPort(Port.ACC) + arg.GetValue(Chip));
            return PCInstruction.INCREMENT;
        }
    }
}