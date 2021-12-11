using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardZero : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(0, _handedness_right);
    }
}
