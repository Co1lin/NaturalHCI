using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureTwo : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTwo(_handedness_right);
    }
}