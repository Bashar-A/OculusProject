using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;

public static class GestureUtils
{
    private const float PinchThreshold = 0.7f;
    private const float CurlThreshold = 0.4f;

    public static InputGesture DetectInputGesture(Handedness trackedHand)
    {
        var isPinching = HandPoseUtils.CalculateIndexPinch(trackedHand) > PinchThreshold;
        var isCurlMiddle = HandPoseUtils.MiddleFingerCurl(trackedHand) > CurlThreshold;
        var isCurlRing = HandPoseUtils.RingFingerCurl(trackedHand) > CurlThreshold;
        var isCurlPinky = HandPoseUtils.PinkyFingerCurl(trackedHand) > CurlThreshold;
        //var isCurlThumb = HandPoseUtils.ThumbFingerCurl(trackedHand) > CurlThreshold;
        
        if (!isPinching && isCurlMiddle && isCurlRing && isCurlPinky)
        {
            return trackedHand.IsRight() ? InputGesture.RightFist : InputGesture.LeftFist;
        }

        if (isPinching)
        {
            return trackedHand.IsRight() ? InputGesture.RightIndex : InputGesture.LeftIndex;
        }
        
        if (isCurlMiddle && !isCurlRing && !isCurlPinky)
        {
            return trackedHand.IsRight() ? InputGesture.RightMiddle : InputGesture.LeftMiddle;
        }
        
        if (!isCurlMiddle && isCurlRing && !isCurlPinky)
        {
            return trackedHand.IsRight() ? InputGesture.RightRing : InputGesture.LeftRing;
        }
        
        if (!isCurlMiddle && !isCurlRing && isCurlPinky)
        {
            return trackedHand.IsRight() ? InputGesture.RightPinky : InputGesture.LeftPinky;
        }

        return InputGesture.Undefined;
    }
}