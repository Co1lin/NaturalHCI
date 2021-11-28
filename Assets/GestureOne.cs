using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureOne : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsOne(_handedness_right);
    }
}
