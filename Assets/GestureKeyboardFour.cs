using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardFour : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardFour(_handedness_left, _handedness_right);
    }
}
