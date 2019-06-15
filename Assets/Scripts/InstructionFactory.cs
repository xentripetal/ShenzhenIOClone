using System;
using System.Linq;
using Zachclone.Instructions;
using Zachclone.Instructions.Models;

namespace Zachclone {
    public static class InstructionFactory {
        public static Instruction CreateInstruction(string[] input, Chip chip, int relativePos, int actualPos, TestPrefix testPrefix) {
            var command = input[0];
            var args = input.Skip(1).ToArray();
            switch (command) {
                case "add":
                    return new AddInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "sub":
                    return new SubInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "mul":
                    return new MulInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "div":
                    return new DivInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "not":
                    return new NotInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "dgt":
                    return new DgtInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "dst":
                    return new DstInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "nop":
                    return new NopInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "mov":
                    return new MovInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "jmp":
                    return new JmpInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "slp":
                    return new SlpInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "slx":
                    throw new NotImplementedException();
                case "teq":
                    return new TeqInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "tgt":
                    return new TgtInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "tlt":
                    return new TltInstruction(chip, args, relativePos, actualPos, testPrefix);
                case "tcp":
                    return new TcpInstruction(chip, args, relativePos, actualPos, testPrefix);
                default:
                    throw new InstructionValidationException("Unknown command");
            }
        }
    }
}