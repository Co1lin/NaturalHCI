using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardTwo : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(2, _handedness_right);
    }
}
