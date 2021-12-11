using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureThumbRight : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsThumbRight(_handedness_right);
    }
}