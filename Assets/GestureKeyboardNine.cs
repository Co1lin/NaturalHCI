using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardNine : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardNine(_handedness_left, _handedness_right);
    }
}
