using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureNine : GestureWidget
{
    public override bool GestureCondition()
    {
        return IsNine(_handedness_right);
    }
}