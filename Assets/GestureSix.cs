using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureSix : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsSix(_handedness_right);
    }
}