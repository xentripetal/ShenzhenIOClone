namespace Instructions.Models {
    public class NoArg {
        public NoArg(string[] args) {
            if (args.Length != 0) throw new InstructionValidationException("Instruction does not take any arguments");
        }
    }
}