using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureZero : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsZero(_handedness_right);
    }
}