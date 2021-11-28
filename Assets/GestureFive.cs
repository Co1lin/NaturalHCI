using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureFive : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsFive(_handedness_right);
    }
}