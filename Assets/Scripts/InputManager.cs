using System;
using System.Text;
using Enums;
using TMPro;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject textGameObject = null;
    [SerializeField] private GameObject logGameObject = null;
    [SerializeField] private GameObject charTableGameObject = null;
    
    private InputGesture _lastGesture;
    private bool _isSameInputLock;
    private bool _typingIsDone;
    private bool _isCaps;
    
    private TextMesh _textMesh;
    private TextMesh _logMesh;
    private TextMeshPro _charTableMesh;

    // ▲►←◌
    private string[,] charTable = new string[10, 10]
    {
        {"п", "р", "c", "d", "e", "f", "g", "h", "", ""},
        {"и", "j", "k", "l", "m", "n", "o", "p", "", ""},
        {"q", "r", "s", "t", "u", "v", "w", "x", "",""},
        {"y", "z", "0", "1", "2", "3", "4", "5", "",""},
        {"в", "е", "т", "!", "", "←","►", "▲", "",""},
        {"", "", "", "", "", "","", "", "",""},
        {"", "", "", "", "", "","", "", "",""},
        {"", "", "", "", "", "","", "", "",""},
        {"", "", "", "", "", "","", "", "",""},
        {"", "", "", "", "", "","", "", "",""}
    };
    
    
    void Start()
    {
        EventManager.OnInputGesture += OnInput;
        _textMesh = textGameObject.GetComponent<TextMesh>();
        _logMesh = logGameObject.GetComponent<TextMesh>();
        _charTableMesh = charTableGameObject.GetComponent<TextMeshPro>();
        DrawCharTable();
        Debug.LogWarning($"InputManager started");
    }

    void OnInput(InputGesture inputGesture)
    {
        if (inputGesture == InputGesture.Undefined)
        {
            _isSameInputLock = false;
            return;
        }

        if (inputGesture == _lastGesture && _isSameInputLock)
        {
            return;
        }


        if (_typingIsDone)
        {
            if (charTable[(int) _lastGesture, (int) inputGesture] == "delete")
            {
                _textMesh.text = _textMesh.text.Substring(0, _textMesh.text.Length - 1);
            }
            else
            {
                _textMesh.text += charTable[(int) _lastGesture, (int) inputGesture];
            }
            _logMesh.text = $"{_lastGesture} + {inputGesture}";
            Debug.LogWarning($"{_lastGesture} + {inputGesture}");
            Debug.LogWarning(charTable[(int) _lastGesture, (int) inputGesture]);
            _typingIsDone = false;
            DrawCharTable();
            return;
        }

        _lastGesture = inputGesture;
        _typingIsDone = true;
        _isSameInputLock = true;
        DrawCharTable((int)inputGesture);
    }

    private void DrawCharTable(int row = -1)
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append("<mspace=mspace=1>");
        stringBuilder.Append("<mark=#FFFFFF80>♦ 0 1 2 3 4 5 6 7 8 9\n</mark>");
        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append($"<mark=#FFFFFF80>{i} </mark>");
            
            if (row == i)
            {
                stringBuilder.Append($"<mark=#FFFF0050>");
            }
            
            for (var j = 0; j < 10; j++)
            {
                var symbol = string.IsNullOrEmpty(charTable[i, j]) ? "◌" : charTable[i, j];
                stringBuilder.Append($"{symbol} ");
            }
            
            if (row == i)
            {
                stringBuilder.Append($"</mark>");
            }
            stringBuilder.Append("\n");
        }
        stringBuilder.Append("</mspace>");

        _charTableMesh.text = stringBuilder.ToString();
    }
}
