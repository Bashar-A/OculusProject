using System.Text;
using Enums;
using Microsoft.MixedReality.Toolkit.Input;
using TMPro;
using UnityEngine;

public class TextEditor : MonoBehaviour, IMixedRealityTouchHandler
{
    private string _clipboardBuffer = string.Empty;
    private TextState _state = TextState.None;
    private int _startIndex = 0;
    private int _endIndex = 0;

    private TextMeshPro _textComponent;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        // _textComponent.ForceMeshUpdate();
        // HighlightText(0, 4);
    }

    #region Events
    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        Debug.LogWarning("OnTouchStarted");
        _startPosition = eventData.InputData;
    }

    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        Debug.LogWarning("OnTouchCompleted");
        _endPosition = eventData.InputData;

        if (TryFindCharacterByPosition(_startPosition, out var startIndex)
            && TryFindCharacterByPosition(_endPosition, out var endIndex))
        {
            Debug.LogWarning($"StartChar: {startIndex}, EndChar: {endIndex}");
            if (startIndex > endIndex)
            {
                (startIndex, endIndex) = (endIndex, startIndex);
            }
            
            if (startIndex == endIndex)
            {
                RemoveHighlight();
                _state =  TextState.PositionSelected;
            }
            else
            {
                HighlightText(startIndex, endIndex);
                _state = TextState.Highlighted;
            }
            _startIndex = startIndex;
            _endIndex = endIndex;
            return;
        }

        _state = TextState.None;
        _startIndex = 0;
        _endIndex = 0;
    }

    public void OnTouchUpdated(HandTrackingInputEventData eventData)
    {
    }

    #endregion
    
    #region Actions
    public void OnCopyAction()
    {
        if (_state != TextState.Highlighted)
        {
            _clipboardBuffer = string.Empty;
            return;
        }

        _clipboardBuffer = GetText(_startIndex, _endIndex);
    }

    public void OnCutAction()
    {
        if (_state != TextState.Highlighted)
        {
            _clipboardBuffer = string.Empty;
            return;
        }
        
        _clipboardBuffer = GetText(_startIndex, _endIndex);
        OnDeleteAction();
    }

    public void OnPasteAction()
    {
        switch (_state)
        {
            case TextState.Highlighted:
                OnDeleteAction();
                AppendText(_clipboardBuffer, _startIndex);
                break;
            case TextState.PositionSelected:
                RemoveHighlight();
                AppendText(_clipboardBuffer, _startIndex);
                break;
            default:
                return;
        }
    }

    public void OnDeleteAction()
    {
        if (_state != TextState.Highlighted)
        {
            return;
        }
        DeleteText(_startIndex, _endIndex);
        _state = TextState.None;
        _startIndex = 0;
        _endIndex = 0;
    }
    
    public void OnResetStateAction()
    {
        RemoveHighlight();
        _state = TextState.None;
        _startIndex = 0;
        _endIndex = 0;
    }
    #endregion
    
    #region TextUtils
    private void RemoveHighlight()
    {
        _textComponent.text = GetTextWithoutTag();
    }
    private string GetTextWithoutTag()
    {
        return _textComponent.text.Replace("<mark=#FFFFFF80>", "").Replace("</mark>", "");
    }

    private bool TryFindCharacterByPosition(Vector3 pos, out int index)
    {
        index = -1;
        var textInfo = _textComponent.textInfo;
        var localPos = _textComponent.transform.InverseTransformPoint(pos);
        // TODO
        var minDistance = float.MaxValue;
        for (var i = 0; i < textInfo.characterInfo.Length; i++)
        {
            var character = textInfo.characterInfo[i];
            if (character.character == '\0')
            {
                continue;
            }

            var distance = Vector3.Distance(
                Vector3.Lerp(character.bottomLeft, character.topLeft, 0.5f),
                localPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }

        return index != -1;
    }

    private void HighlightText(int start, int end)
    {
        if (_textComponent == null)
            return;

        if (start > end)
        {
            (start, end) = (end, start);
        }

        if (start < 0 || end >= _textComponent.textInfo.characterInfo.Length)
            return;

        var tempText = GetTextWithoutTag();
        var stringBuilder = new StringBuilder(tempText.Substring(0, start));
        stringBuilder.Append("<mark=#FFFFFF80>");
        stringBuilder.Append(tempText.Substring(start, end - start));
        stringBuilder.Append("</mark>");
        stringBuilder.Append(tempText.Substring(end));

        _textComponent.text = stringBuilder.ToString();
    }

    private string GetText(int startIndex, int endIndex)
    {
        return GetTextWithoutTag().Substring(startIndex, endIndex - startIndex);
    }

    private void AppendText(string text, int startIndex)
    {
        var tempText = GetTextWithoutTag();
        var stringBuilder = new StringBuilder(tempText.Substring(0, startIndex));
        stringBuilder.Append(text);
        stringBuilder.Append(tempText.Substring(startIndex));

        _textComponent.text = stringBuilder.ToString();
    }

    private void DeleteText(int startIndex, int endIndex)
    {
        _textComponent.text = GetTextWithoutTag().Remove(startIndex, endIndex - startIndex);
    }
    #endregion

}
