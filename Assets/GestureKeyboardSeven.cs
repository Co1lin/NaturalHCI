using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardSeven : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(7, _handedness_right);
    }
}
