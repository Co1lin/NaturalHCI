using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardTwo : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardTwo(_handedness_left, _handedness_right);
    }
}
