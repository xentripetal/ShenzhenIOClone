namespace Instructions.Models {
    public class DoubleRIRArg {
        public SingleRIArg firstArg;
        public SingleRArg secondArg;

        public DoubleRIRArg(string[] args, Chip chip) {
            if (args.Length != 2)
                throw new InstructionValidationException("Instruction must take two values of integer or port");
            firstArg = new SingleRIArg(new[] {args[0]}, chip);
            secondArg = new SingleRArg(new[] {args[1]}, chip);
        }

        public (int, int) GetValues(Chip chip) {
            return (firstArg.GetValue(chip), secondArg.GetValue(chip));
        }
    }
}