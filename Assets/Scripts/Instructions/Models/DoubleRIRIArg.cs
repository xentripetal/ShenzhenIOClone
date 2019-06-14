namespace Instructions.Models {
    public class DoubleRIRIArg {
        public SingleRIArg firstArg;
        public SingleRIArg secondArg;

        public DoubleRIRIArg(string[] args, Chip chip) {
            if (args.Length != 2)
                throw new InstructionValidationException("Instruction must take two values of integer or port");
            firstArg = new SingleRIArg(new[] {args[0]}, chip);
            secondArg = new SingleRIArg(new[] {args[1]}, chip);
        }

        public (int, int) GetValues(Chip chip) {
            return (firstArg.GetValue(chip), secondArg.GetValue(chip));
        }
    }
}