using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardEight : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(8, _handedness_right);
    }
}
