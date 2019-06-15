namespace Zachclone.Instructions.Models {
    public class LabelArg {
        public string Label;

        public LabelArg(string[] args, Chip chip) {
            if (args.Length != 1) throw new InstructionValidationException("Must take a single label arg");

            if (!chip.HasLabel(args[0]))
                throw new InstructionValidationException($"The label {args[0]} does not exist.");

            Label = args[0];
        }
    }
}