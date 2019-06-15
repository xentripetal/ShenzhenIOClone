using System;
using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class DgtInstruction : Instruction {
        private RIArg arg;
        public DgtInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            arg = new RIArg(args, chip);
        }
        
        public override PCInstruction Execute() {
            var pos = arg.GetValue(Chip);
            var acc = Chip.ReadPort(Port.ACC);
            if (pos < 3 || pos > -1) Chip.WritePort(Port.ACC, (int) (acc / Math.Pow(10, pos) % 10));
            return PCInstruction.INCREMENT;
        }
    }
}