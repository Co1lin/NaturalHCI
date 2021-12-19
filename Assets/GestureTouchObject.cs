using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureTouchObject : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTouchObject(_handedness_left, _target);
    }
}
