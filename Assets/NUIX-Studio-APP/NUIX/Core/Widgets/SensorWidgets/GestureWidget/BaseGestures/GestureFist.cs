using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureFist : GestureWidget
{
    public override bool GestureCondition()
    {
        return HandPoseUtils.IsThumbGrabbing(_handedness_right) && 
            HandPoseUtils.IsMiddleGrabbing(_handedness_right) && 
            HandPoseUtils.IsIndexGrabbing(_handedness_right) &&
            IsPinkyGrabbing(_handedness_right) &&
            IsRingGrabbing(_handedness_right);
    }
}
