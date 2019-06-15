using System;

namespace Zachclone.Instructions.Models {
    public class RArg {
        public Port Port;
        
        public RArg(string arg, Chip chip) {
            if (!Enum.TryParse(arg.ToUpper(), out Port port))
                throw new InstructionValidationException("Provided value is not a valid port.");

            if (!chip.HasPort(port)) throw new InstructionValidationException("The specified port is not valid.");

            Port = port;
        }

        public int GetValue(Chip chip) {
            return chip.ReadPort(Port);
        }
    }
}