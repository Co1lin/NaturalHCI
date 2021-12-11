using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureTheForceUp : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTheForceUp(_handedness_right, _target);
    }
}
