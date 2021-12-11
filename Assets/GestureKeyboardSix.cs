using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardSix : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboardSix(_handedness_left, _handedness_right);
    }
}
