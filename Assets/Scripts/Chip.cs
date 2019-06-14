using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Instructions;
using Instructions.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour {
    private static readonly Dictionary<string, IInstruction> validInstructions = new Dictionary<string, IInstruction> {
        {"add", new Add_Instruction()},
        {"sub", new Sub_Instruction()},
        {"div", new Div_Instruction()},
        {"mul", new Mul_Instruction()},
        {"dgt", new Dgt_Instruction()},
        {"not", new Not_Instruction()},
        {"nop", new Nop_Instruction()},
        {"swp", new Swp_Instruction()},
        {"mov", new Mov_Instruction()},
        {"dst", new Dst_Instruction()},
        {"jmp", new Jmp_Instruction()},
        {"teq", new Teq_Instruction()},
        {"tgt", new Tgt_Instruction()},
        {"tlt", new Tlt_Instruction()},
        {"tcp", new Tcp_Instruction()}
    };

    private int _accumulator;

    private List<StoredInstruction> _instructions;
    private Dictionary<string, int> _labels;

    private int _dataRegister;
    private TestPrefix _testOutcome = TestPrefix.NONE;
    public TMP_Text AccText;
    public TMP_InputField InstructionSource;

    public int CurrentTick;
    public bool IsSleeping;


    public string[] InstructionText;
    public RectTransform PCLineRect;
    public TMP_Text DatText;
    public Toggle StartToggle;
    public Button StepButton;

    public int Accumulator {
        get => _accumulator;
        set {
            _accumulator = Mathf.Clamp(value, Constants.INT_MIN, Constants.INT_MAX);
            AccText.text = "" + _accumulator;
        }
    }

    public int DataRegister {
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
        if (port == Port.ACC) {
            Accumulator = value;
        } else if (port == Port.DAT) {
            DataRegister = value;
        }
    }

    public bool HasPort(Port port) {
        return true;
    }

    public bool HasLabel(string label) {
        if (_labels.ContainsKey(label)) {
            return true;
        }

        foreach (var instrText in InstructionText) {
            if (instrText.Contains(label + ":")) {
                return true;
            }
        }

        return false;
    }

    public void GoToLabel(string label) {
        PC = _labels[label];
        PCLineRect.anchoredPosition = new Vector2(0, -14f - 17.5f * PC);
    }

    public void OnStartChanged(bool value) {
        if (value && TryInitialize())
            Enable();
        else
            Disable();
    }

    private void Enable() {
        InstructionSource.interactable = false;
        StepButton.interactable = true;
        PCLineRect.gameObject.SetActive(true);
        PC = 0;
        SetValidPC();
    }

    private void Disable() {
        InstructionSource.interactable = true;
        Accumulator = 0;
        DataRegister = 0;
        _testOutcome = TestPrefix.NONE;
        StepButton.interactable = false;
        PCLineRect.gameObject.SetActive(false);
        StartToggle.SetIsOnWithoutNotify(false);
    }

    private bool TryInitialize() {
        InstructionText = InstructionSource.text.Replace("\r", "").Split('\n');
        _instructions = new List<StoredInstruction>();
        _labels = new Dictionary<string, int>();

        // i = visible line number
        // x = relative line number
        var x = 0;
        for (var i = 0; i < InstructionText.Length; i++) {
            var parts = InstructionText[i].SkipWhile(c => c == ' ').ToArray().ArrayToString().Split(' ');

            var labelSplit = parts[0].Split(':');
            if (labelSplit.Length == 2) {
                var label = labelSplit[0];
                if (labelSplit[1] != "")
                    parts[0] = labelSplit[1];
                else
                    parts = parts.Skip(1).ToArray();

                if (_labels.ContainsKey(label)) {
                    Debug.LogError($"Label {label} already exists.");
                    return false;
                }

                _labels.Add(label, x);
            }

            if (parts.Length == 0 || parts[0] == null || parts[0] == "") continue;

            var storedInstruction = new StoredInstruction();
            storedInstruction.TestPrefix = TestPrefix.NONE;

            if (parts[0][0] == '+')
                storedInstruction.TestPrefix = TestPrefix.POSITIVE;
            else if (parts[0][0] == '-') storedInstruction.TestPrefix = TestPrefix.NEGATIVE;

            if (storedInstruction.TestPrefix != TestPrefix.NONE) {
                if (parts[0].Length == 1)
                    parts = parts.Skip(1).ToArray();
                else
                    parts[0] = parts[0].Substring(1);
            }

            // Check if instruction exists
            if (!validInstructions.TryGetValue(parts[0], out var instruction)) {
                Debug.LogError("Unknown command on line " + i);
                return false;
            }

            storedInstruction.Instruction = instruction;

            try {
                storedInstruction.Argument = instruction.GenerateArgObj(this, parts.Skip(1).ToArray());
                storedInstruction.ActualPos = i;
                storedInstruction.RelativePos = x;
                _instructions.Add(storedInstruction);
                x++;
            } catch (InstructionValidationException e) {
                Debug.LogError($"Line {i}: {e.Message}");
                return false;
            }
        }

        return true;
    }

    private void SetValidPC() {
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

        PCLineRect.anchoredPosition = new Vector2(0, -14f - 17.5f * storedInstr.ActualPos);
    }

    public void Step() {
        if (_instructions.Count == 0) return;
        var storedInstr = _instructions[PC];
        storedInstr.Execute(this);

        if (storedInstr.Instruction.GetPCInstruction() == PCInstruction.INCREMENT) IncrementPC();
        SetValidPC();
    }

    private void IncrementPC() {
        PC++;
        if (PC >= _instructions.Count) PC = 0;
    }
}