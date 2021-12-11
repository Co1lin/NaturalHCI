using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardOne : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardOne(_handedness_left, _handedness_right);
    }
}
