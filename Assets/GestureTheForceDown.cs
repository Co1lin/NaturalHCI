using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureTheForceDown : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTheForceDown(_handedness_right, _target);
    }
}
