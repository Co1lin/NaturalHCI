using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureThree : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsThree(_handedness_right);
    }
}