using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureSeven : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsSeven(_handedness_right);
    }
}