using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardSix : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(6, _handedness_right);
    }
}
