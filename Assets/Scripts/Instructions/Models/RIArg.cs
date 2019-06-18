using System;

namespace Zachclone.Instructions.Models {
    public class RIArg {
        public int Int;
        public bool IsInt;
        public Register Register;

        public RIArg(string arg, Chip chip) {
            Initialize(arg, chip);
        }

        public RIArg(string[] args, Chip chip) {
            if (args.Length != 1) throw new InstructionValidationException("must take a single integer or port");
            Initialize(args[0], chip);
        }

        private void Initialize(string arg, Chip chip) {
            if (!int.TryParse(arg, out var inc)) {
                if (!Enum.TryParse(arg.ToUpper(), out Register port))
                    throw new InstructionValidationException("Provided value is not an integer or a valid port.");

                if (!chip.HasPort(port)) throw new InstructionValidationException("The specified port is not valid.");

                Register = port;
                IsInt = false;
                return;
            }

            if (inc > Constants.INT_MAX || inc < Constants.INT_MIN)
                throw new InstructionValidationException("Provided integer is out of bounds.");

            Int = inc;
            IsInt = true;
            
        }

        public int GetValue(Chip chip) {
            return IsInt ? Int : chip.ReadPort(Register);
        }
    }
}