using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureThumbsUp : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsThumbUp(_handedness_right);
    }
}
