using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureThumbUp : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsThumbUp(_handedness_right);
    }
}
