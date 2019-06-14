using System;
using DefaultNamespace;

namespace Instructions.Models {
    public class SingleRArg {
        public Port Port;

        public SingleRArg(string[] args, Chip chip) {
            if (args.Length != 1) throw new InstructionValidationException("must take a single port arg");

            if (!Enum.TryParse(args[0].ToUpper(), out Port port))
                throw new InstructionValidationException("Provided value is not a valid port.");

            if (!chip.HasPort(port)) throw new InstructionValidationException("The specified port is not valid.");

            Port = port;
        }

        public int GetValue(Chip chip) {
            return chip.ReadPort(Port);
        }
    }
}