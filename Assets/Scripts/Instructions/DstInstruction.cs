using System;
using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class DstInstruction : Instruction {
        private RIArg firstArg;
        private RIArg secondArg;
        
        public DstInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            if (args.Length != 2) {
                throw new InstructionValidationException("dst requires 2 args");
            }

            firstArg = new RIArg(args[0], chip);
            secondArg = new RIArg(args[1], chip);
        }

        public override PCInstruction Execute() {
            var pos = firstArg.GetValue(Chip);
            if (pos > 2 || pos < 0) return PCInstruction.INCREMENT;

            pos = 2 - pos;
            var acc = Chip.ReadPort(Port.ACC);
            var isNegative = acc < 0;
            var accStr = Math.Abs(acc).ToString();
            var newValue = (secondArg.GetValue(Chip) % 10).ToString();

            accStr = accStr.PadLeft(3, '0');

            acc = int.Parse(accStr.Remove(pos, 1).Insert(pos, newValue));
            if (isNegative) acc *= -1;

            Chip.WritePort(Port.ACC, acc);
            return PCInstruction.INCREMENT;
        }
    }
}