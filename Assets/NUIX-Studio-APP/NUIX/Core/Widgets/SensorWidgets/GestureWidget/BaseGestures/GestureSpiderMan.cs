using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureSpiderMan : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTwoHandsNear(_handedness_left, _handedness_right);
    }
}