using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zachclone.Instructions.Models;

namespace Zachclone {
    public class Chip : MonoBehaviour {
        public TMP_Text AccText;
        public TMP_Text DatText;
        public TMP_InputField InstructionSource;
        public RectTransform PCLineRect;

        public string[] InstructionText;
        public bool IsSleeping;

        private List<Instruction> _instructions;
        private Dictionary<string, int> _labels;
        private TestPrefix _testOutcome = TestPrefix.NONE;

        private Dictionary<Register, Connection> _Connections = new Dictionary<Register, Connection>() {
            {Register.P0, new Connection(ConnectionType.SIMPLE)},
            {Register.P1, new Connection(ConnectionType.SIMPLE)},
            {Register.X0, new Connection(ConnectionType.XBUS)},
            {Register.X1, new Connection(ConnectionType.XBUS)},
            {Register.X2, new Connection(ConnectionType.XBUS)},
            {Register.X3, new Connection(ConnectionType.XBUS)}
        };

        private int PC {
            get => _pc;
            set {
                _pc = value;
                if (_pc >= _instructions.Count) {
                    _pc = 0;
                }
            }
        }

        private int _pc;

        private int Accumulator {
            get => _accumulator;
            set {
                _accumulator = Mathf.Clamp(value, Constants.INT_MIN, Constants.INT_MAX);
                AccText.text = "" + _accumulator;
            }
        }

        private int _accumulator;

        private int DataRegister {
            get => _dataRegister;
            set {
                _dataRegister = Mathf.Clamp(value, Constants.INT_MIN, Constants.INT_MAX);
                DatText.text = "" + _dataRegister;
            }
        }

        private int _dataRegister;

        public void SetTestOutcome(TestPrefix testPrefix) {
            _testOutcome = testPrefix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="register"></param>
        /// <returns>Value of port or -1000 if blocking</returns>
        public int ReadPort(Register register) {
            if (register == Register.ACC)
                return Accumulator;
            if (register == Register.DAT)
                return DataRegister;

            return _Connections[register].Read();
        }

        public WritePortResponse WritePort(Register register, int value) {
            var response = WritePortResponse.WRITTEN;
            if (register == Register.ACC)
                Accumulator = value;
            else if (register == Register.DAT)
                DataRegister = value;
            else {
                var conn = _Connections[register];
                if (conn.ConnectionType == ConnectionType.SIMPLE || conn.IsNonBlocking) {
                    conn.Write(value);
                }
                // If not active writer and no other active writer, register 
                // if not active writer and other active writer, wait
                // if previously wrote value, wait if still is active writer, else increment
                else {
                    if (!conn.IsActiveWriter) {
                        if (!conn.Wire.HasActiveWriter) {
                            conn.Write(value);
                            response = WritePortResponse.REGISTERED;
                        }
                        else {
                            response = WritePortResponse.WAITING;
                        }
                    }
                    else {
                        if (!conn.Wire.HasActiveReader) {
                            response = WritePortResponse.WAITING;
                        }
                        conn.Write(value);
                    }
                }
            }

            return response;
        }

        public bool HasPort(Register register) {
            if (register == Register.ACC || register == Register.DAT) {
                return true;
            }

            var found = _Connections.TryGetValue(register, out var connection);
            return found && connection.IsWired();
        }

        public bool HasLabel(string label) {
            if (_labels.ContainsKey(label)) return true;

            return InstructionText.Any(instrText => instrText.Contains(label + ":"));
        }

        public void GoToLabel(string label) {
            PC = _labels[label];
            PCLineRect.anchoredPosition = new Vector2(0, -14f - 17.5f * PC);
        }

        public void WireRegister(Wire wire, Register register) {
            _Connections[register].SetWire(wire);
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
            label = string.Empty;
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
                }
                catch (InstructionValidationException e) {
                    Debug.LogError($"Line {i}: {e}");
                    return false;
                }
            }

            return true;
        }

        private bool SetValidPC() {
            if (_instructions.Count == 0) return false;
            var currentPc = PC;
            var storedInstr = _instructions[PC];
            while (true) {
                if (storedInstr.TestPrefix == TestPrefix.NONE || storedInstr.TestPrefix == _testOutcome) break;
                PC++;
                if (PC == currentPc) {
                    Debug.LogError("Infinite Loop Detected");
                    return false;
                }

                storedInstr = _instructions[PC];
            }

            PCLineRect.anchoredPosition = new Vector2(0, -13f * storedInstr.ActualPos);
            return true;
        }

        public void Step() {
            if (_instructions.Count == 0) return;
            var instr = _instructions[PC];
            var pcInstr = instr.Execute();

            if (pcInstr != PCInstruction.NOTHING) PC++;
            SetValidPC();
            if (pcInstr == PCInstruction.RUN_NEXT) {
                Step();
            }
        }
    }
}