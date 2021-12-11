using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardSeven : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardSeven(_handedness_left, _handedness_right);
    }
}
