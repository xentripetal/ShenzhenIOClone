using System;
using DefaultNamespace;

namespace Instructions.Models {
    public class SingleRIArg {
        public int Int;
        public bool IsInt;
        public Port Port;

        public SingleRIArg(string[] args, Chip chip) {
            if (args.Length != 1) throw new InstructionValidationException("must take a single integer or port");

            if (!int.TryParse(args[0], out var inc)) {
                if (!Enum.TryParse(args[0].ToUpper(), out Port port))
                    throw new InstructionValidationException("Provided value is not an integer or a valid port.");

                if (!chip.HasPort(port)) throw new InstructionValidationException("The specified port is not valid.");

                Port = port;
                IsInt = false;
                return;
            }

            if (inc > Constants.INT_MAX || inc < Constants.INT_MIN)
                throw new InstructionValidationException("Provided integer is out of bounds.");

            Int = inc;
            IsInt = true;
        }

        public int GetValue(Chip chip) {
            return IsInt ? Int : chip.ReadPort(Port);
        }
    }
}