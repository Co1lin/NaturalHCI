using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureTheForce : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTheForce(_handedness_right, _target);
    }
}
