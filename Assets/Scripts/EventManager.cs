using System.Threading.Tasks;

public class EventManager
{
    public delegate void OnInputGestureDelegate(InputGesture inputGesture);
    public static event OnInputGestureDelegate OnInputGesture;
    
    private static bool _canSendOnInputGestureEvent = true;

    public static void RaiseOnInputGesture(InputGesture inputGesture)
    {
        if (!_canSendOnInputGestureEvent)
        {
            return;
        }

        _canSendOnInputGestureEvent = false;
        OnInputGesture?.Invoke(inputGesture);
        if (inputGesture == InputGesture.Undefined)
        {
            _canSendOnInputGestureEvent = true;
        }
        else
        {
            Task.Delay(400).ContinueWith(_ => _canSendOnInputGestureEvent = true);   
        }
    }
}