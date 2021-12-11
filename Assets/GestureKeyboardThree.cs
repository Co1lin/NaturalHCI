using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureKeyboardThree : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsKeyboard(3, _handedness_right);
    }
}
