using System;

namespace Instructions.Models {
    public class InstructionValidationException : Exception {
        public InstructionValidationException(string message) : base(message) {
        }
    }
}