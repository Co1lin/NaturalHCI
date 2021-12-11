using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardZero : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardZero(_handedness_left, _handedness_right);
    }
}
