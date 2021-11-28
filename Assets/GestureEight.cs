using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureEight : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsEight(_handedness_right);
    }
}