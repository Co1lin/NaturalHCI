using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardThree : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardThree(_handedness_left, _handedness_right);
    }
}
