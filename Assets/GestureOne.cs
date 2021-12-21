using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureOne : GestureWidget
{
    public override void Start() {
        _audioSource.enabled = false;
    }
    public override bool GestureCondition()
    {
        return IsOne(_handedness_right);
    }
}
