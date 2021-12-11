using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureThumbDown : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsThumbDown(_handedness_right);
    }
}