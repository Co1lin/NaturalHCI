using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardNine : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(9, _handedness_right);
    }
}
