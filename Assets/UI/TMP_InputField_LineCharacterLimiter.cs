using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMP_InputField_LineCharacterLimiter : MonoBehaviour {
    public int LineCharacterLimit;
    public int LineLimit;
    public TMP_InputField TmpInputField;

    public void OnValueChanged(string str) {
        if (LineCharacterLimit == 0 && LineLimit == 0) return;

        var invalid = false;
        var lines = str.Split('\n');
        var pickedLines = new List<string>();
        for (var i = 0; i < lines.Length; i++) {
            if (LineLimit != 0 && i >= LineLimit) {
                var roomMade = false;
                for (var x = pickedLines.Count - 1; x >= 0; x--)
                    if (string.IsNullOrWhiteSpace(pickedLines[x])) {
                        pickedLines.RemoveAt(x);
                        roomMade = true;
                        break;
                    }

                invalid = true;
                if (!roomMade) continue;
            }

            var line = lines[i];
            if (LineCharacterLimit != 0 && line.Length > LineCharacterLimit) {
                invalid = true;
                line = line.Substring(0, LineCharacterLimit);
            }

            pickedLines.Add(line);
        }

        if (invalid) TmpInputField.text = string.Join("\n", pickedLines);
    }
}