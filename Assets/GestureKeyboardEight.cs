using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardEight : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardEight(_handedness_left, _handedness_right);
    }
}
