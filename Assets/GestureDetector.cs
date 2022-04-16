using System;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureDetector : MonoBehaviour
{
    private void Update()
    {
        var leftInput = GestureUtils.DetectInputGesture(Handedness.Left);
        var rightInput = GestureUtils.DetectInputGesture(Handedness.Right);

        if (leftInput != InputGesture.Undefined)
        {
            EventManager.RaiseOnInputGesture(leftInput);
        }
        else
        {
            EventManager.RaiseOnInputGesture(rightInput);
        }
    }
}
