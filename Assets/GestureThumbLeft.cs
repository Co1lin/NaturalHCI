using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureThumbLeft : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsThumbLeft(_handedness_right);
    }
}