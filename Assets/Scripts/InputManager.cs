using System;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject textGameObject = null;
    [SerializeField] private GameObject logGameObject = null;
    
    private InputGesture _lastGesture;
    private bool _isSameInputLock;
    private bool _typingIsDone;
    private bool _isCaps;
    
    private TextMesh _textMesh;
    private TextMesh _logMesh;

    private string[,] charTable = new string[10, 10]
    {
        {"п", "р", "c", "d", "e", "f", "g", "h", "", ""},
        {"и", "j", "k", "l", "m", "n", "o", "p", "", ""},
        {"q", "r", "s", "t", "u", "v", "w", "x", "",""},
        {"y", "z", "0", "1", "2", "3", "4", "5", "",""},
        {"в", "е", "т", "!", "", "delete","", "", "",""},
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
            return;
        }

        _lastGesture = inputGesture;
        _typingIsDone = true;
        _isSameInputLock = true;
    }
}
