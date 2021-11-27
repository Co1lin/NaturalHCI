using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureClap : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsTwoHandsNear(_handedness_left, _handedness_right);
    }
}
