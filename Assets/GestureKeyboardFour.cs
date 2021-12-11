using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardFour : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(4, _handedness_right);
    }
}
