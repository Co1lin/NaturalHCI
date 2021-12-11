using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardFive : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(5, _handedness_right);
    }
}
