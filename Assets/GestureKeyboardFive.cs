using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardFive : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardFive(_handedness_left, _handedness_right);
    }
}
