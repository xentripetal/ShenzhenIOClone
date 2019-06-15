using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zachclone.Instructions.Models;

namespace Zachclone {
    public class Chip : MonoBehaviour {
        private int _accumulator;

        private int _dataRegister;

        private List<Instruction> _instructions;
        private Dictionary<string, int> _labels;
        private TestPrefix _testOutcome = TestPrefix.NONE;
        public TMP_Text AccText;

        public TMP_Text DatText;
        public TMP_InputField InstructionSource;


        public string[] InstructionText;
        public bool IsSleeping;
        public RectTransform PCLineRect;

        private int Accumulator {
            get => _accumulator;
            set {
                _accumulator = Mathf.Clamp(value, Constants.INT_MIN, Constants.INT_MAX);
                AccText.text = "" + _accumulator;
            }
        }

        private int DataRegister {
            get => _dataRegister;
            set {
                _dataRegister = Mathf.Clamp(value, Constants.INT_MIN, Constants.INT_MAX);
                DatText.text = "" + _dataRegister;
            }
        }

        private int PC { get; set; }

        public void SetTestOutcome(TestPrefix testPrefix) {
            _testOutcome = testPrefix;
        }

        public int ReadPort(Port port) {
            if (port == Port.ACC)
                return Accumulator;
            if (port == Port.DAT) return DataRegister;

            return 0;
        }

        public void WritePort(Port port, int value) {
            if (port == Port.ACC)
                Accumulator = value;
            else if (port == Port.DAT) DataRegister = value;
        }

        public bool HasPort(Port port) {
            return true;
        }

        public bool HasLabel(string label) {
            if (_labels.ContainsKey(label)) return true;

            return InstructionText.Any(instrText => instrText.Contains(label + ":"));
        }

        public void GoToLabel(string label) {
            PC = _labels[label];
            PCLineRect.anchoredPosition = new Vector2(0, -14f - 17.5f * PC);
        }

        public bool Enable() {
            if (!TryInitialize()) return false;
            InstructionSource.interactable = false;
            PCLineRect.gameObject.SetActive(true);
            PC = 0;
            SetValidPC();
            return true;
        }

        public void Disable() {
            InstructionSource.interactable = true;
            Accumulator = 0;
            DataRegister = 0;
            _testOutcome = TestPrefix.NONE;
            PCLineRect.gameObject.SetActive(false);
        }

        private void ExtractLineData(string line, out string[] parts, out string label, out TestPrefix testPrefix) {
            line = line.TakeWhile(c => c != '#').ToArray().ArrayToString(); // Filter comments
            parts = line.Split(' '); // Filter padding
            parts = parts.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(); // Trim empty

            // Get label and remove from instruction parts
            label = String.Empty;
            var labelSplit = parts[0].Split(':');
            if (labelSplit.Length == 2) {
                label = labelSplit[0];
                if (labelSplit[1] != "")
                    parts[0] = labelSplit[1];
                else
                    parts = parts.Skip(1).ToArray();
            }

            testPrefix = TestPrefix.NONE;
            // if line was only a label, exit
            if (parts.Length == 0 || parts[0] == null || parts[0] == "") return;


            // Check for test instruction and remove from parts
            if (parts[0][0] == '+')
                testPrefix = TestPrefix.POSITIVE;
            else if (parts[0][0] == '-') testPrefix = TestPrefix.NEGATIVE;

            if (testPrefix != TestPrefix.NONE) {
                if (parts[0].Length == 1)
                    parts = parts.Skip(1).ToArray();
                else
                    parts[0] = parts[0].Substring(1);
            }
        }

        private bool TryInitialize() {
            InstructionText = InstructionSource.text.Replace("\r", "").Split('\n');
            _instructions = new List<Instruction>();
            _labels = new Dictionary<string, int>();

            // i = actual line number
            // x = relative line number
            var x = 0;
            for (var i = 0; i < InstructionText.Length; i++) {
                try {
                    var line = InstructionText[i];
                    ExtractLineData(line, out var parts, out var label, out var testPrefix);
                    if (!string.IsNullOrWhiteSpace(label)) {
                        if (_labels.ContainsKey(label)) {
                            throw new InstructionValidationException("Label already exists");
                        }
                        _labels.Add(label, x);
                    }
                    if (parts.Length == 0 || parts[0] == null || parts[0] == "") continue;
                    
                    _instructions.Add(InstructionFactory.CreateInstruction(parts, this, x, i, testPrefix));
                    x++;
                } catch (InstructionValidationException e) {
                    Debug.LogError($"Line {i}: {e.Message}");
                    return false;
                }
            }

            return true;
        }

        private void SetValidPC() {
            if (_instructions.Count == 0) return;
            var currentPc = PC;
            var storedInstr = _instructions[PC];
            while (true) {
                if (storedInstr.TestPrefix == TestPrefix.NONE || storedInstr.TestPrefix == _testOutcome) break;
                IncrementPC();
                if (PC == currentPc) {
                    Debug.LogError("Infinite Loop Detected");
                    return;
                }

                storedInstr = _instructions[PC];
            }

            PCLineRect.anchoredPosition = new Vector2(0, -13f * storedInstr.ActualPos);
        }

        public void Step() {
            if (_instructions.Count == 0) return;
            var storedInstr = _instructions[PC];
            var pcInstr = storedInstr.Execute();

            if (pcInstr != PCInstruction.NOTHING) IncrementPC();
            SetValidPC();
            if (pcInstr == PCInstruction.RUN_NEXT) {
                Step();
            }
        }

        private void IncrementPC() {
            PC++;
            if (PC >= _instructions.Count) PC = 0;
        }
    }
}