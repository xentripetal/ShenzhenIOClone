using Instructions.Models;

public interface IInstruction {
    PCInstruction GetPCInstruction();
    string GetInstructionPrefix();
    object GenerateArgObj(Chip chip, string[] args);
    void Execute(Chip chip, object argObj);
}