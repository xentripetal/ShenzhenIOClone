using System;

namespace Zachclone.Instructions.Models {
    public class RArg {
        public Register Register;
        
        public RArg(string arg, Chip chip) {
            if (!Enum.TryParse(arg.ToUpper(), out Register port))
                throw new InstructionValidationException("Provided value is not a valid port.");

            if (!chip.HasPort(port)) throw new InstructionValidationException("The specified port is not valid.");

            Register = port;
        }

        public int GetValue(Chip chip) {
            return chip.ReadPort(Register);
        }
    }
}