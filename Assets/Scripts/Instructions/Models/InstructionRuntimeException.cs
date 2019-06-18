using System;

namespace Zachclone.Instructions.Models {
    public class InstructionRuntimeException : Exception {
        public InstructionRuntimeException(string message) : base(message) {
        }
    }
}