using Zachclone.Instructions.Models;

namespace Zachclone.Instructions {
    public class SlpInstruction : Instruction {
        private RIArg arg;
        private int _sleepTime = 0;
        private bool _initialized = false;

        public SlpInstruction(Chip chip, string[] args, int relativePos, int actualPos, TestPrefix testPrefix) : base(chip, relativePos, actualPos, testPrefix) {
            arg = new RIArg(args, chip);
        }

        public override PCInstruction Execute() {
            if (!_initialized) {
                _sleepTime = arg.GetValue(Chip);
                _initialized = true;
            }

            if (_sleepTime > 0) {
                _sleepTime--;
                Chip.IsSleeping = true;
                return PCInstruction.NOTHING;
            }

            Chip.IsSleeping = false;
            _initialized = false;
            return PCInstruction.INCREMENT;
        }
    }
}