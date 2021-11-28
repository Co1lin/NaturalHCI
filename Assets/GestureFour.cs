using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureFour : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsFour(_handedness_right);
    }
}