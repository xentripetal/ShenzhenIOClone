namespace Zachclone.Instructions.Models {
    public abstract class Instruction {
        public Chip Chip;
        public int RelativePos;
        public int ActualPos;
        public TestPrefix TestPrefix;

        public Instruction(Chip chip, int relativePos, int actualPos, TestPrefix testPrefix) {
            Chip = chip;
            RelativePos = relativePos;
            ActualPos = actualPos;
            TestPrefix = testPrefix;
        }
        public abstract PCInstruction Execute();
    }
}