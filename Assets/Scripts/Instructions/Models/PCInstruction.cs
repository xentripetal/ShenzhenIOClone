namespace Zachclone.Instructions.Models {
    public enum PCInstruction {
        INCREMENT, //Add 1 to PC and wait for next step
        NOTHING, //Wait for next step
        RUN_NEXT //Immediately run the next valid instruction
    }
}