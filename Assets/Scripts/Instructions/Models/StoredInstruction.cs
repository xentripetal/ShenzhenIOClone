namespace Instructions.Models {
    public struct StoredInstruction {
        public IInstruction Instruction;
        public object Argument;
        public int RelativePos;
        public int ActualPos;
        public TestPrefix TestPrefix;

        public StoredInstruction(IInstruction instruction, object argument, int relativePos, int actualPos,
            TestPrefix testPrefix) {
            Instruction = instruction;
            Argument = argument;
            RelativePos = relativePos;
            ActualPos = actualPos;
            TestPrefix = testPrefix;
        }

        public void Execute(Chip chip) {
            Instruction.Execute(chip, Argument);
        }
    }
}