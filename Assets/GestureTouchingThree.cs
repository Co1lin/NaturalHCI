using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureTouchingThree : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTouchingThree(_handedness_left, _handedness_right);
    }
}
