using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardOne : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(1, _handedness_right);
    }
}
