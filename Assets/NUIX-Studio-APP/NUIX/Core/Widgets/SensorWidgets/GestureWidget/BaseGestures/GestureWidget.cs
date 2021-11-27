using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using UnityEngine;

/// <summary>
/// One hand gesture basic class
/// </summary>
public abstract class GestureWidget : Sensor
{
    public Handedness _handedness_left, _handedness_right;

    private DateTime gestureStartTime;

    private bool countDownStarted { get; set; }

    public GestureWidget(Handedness handedness_left = Handedness.Left, Handedness handedness_right = Handedness.Right)
    {
        _handedness_left = handedness_left;
        _handedness_right = handedness_right;
    }


    /// <summary>
    /// Check the requirements, ex. which fingers should be grabbing
    /// </summary>
    /// <returns></returns>
    public abstract bool GestureCondition();
    
    /// <summary>
    /// The value to assign for gesture
    /// Override in your gesture class
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool TryGetGestureValue(out float value)
    {
        value = 0f;
        return false;
    }


    /// <summary>
    /// Trigger virtual sensors based on the Gesture condition
    /// </summary>
    public void GestureEventTrigger()
    {
        if (GestureCondition())
        {
            if (countDownStarted == false)
            {
                gestureStartTime = DateTime.Now;
                countDownStarted = true;
            }
            if ((DateTime.Now.Subtract(gestureStartTime).TotalMilliseconds) > 200)
            {
                SensorTrigger();
            }
        }
        else
        {
            SensorUntrigger();
            countDownStarted = false;
        }
    }

    public override void Start()
    {
        base.Start();

    }

    /// <summary>
    /// Update is called every frame
    /// Check the gesture 
    /// </summary>
    public override void Update()
    {
        base.Update();
        GestureEventTrigger();
    }



    /// <summary>
    /// Returns true if pinky finger tip is closer to wrist than pinky knuckle joint.
    /// </summary>
    /// <param name="hand">Hand to query joint pose against.</param>
    protected bool IsPinkyGrabbing(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out var wristPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, hand, out var indexTipPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, hand, out var indexKnucklePose))
        {
            // compare wrist-knuckle to wrist-tip
            Vector3 wristToIndexTip = indexTipPose.Position - wristPose.Position;
            Vector3 wristToIndexKnuckle = indexKnucklePose.Position - wristPose.Position;
            return wristToIndexKnuckle.sqrMagnitude >= wristToIndexTip.sqrMagnitude;
        }
        return false;
    }

    /// <summary>
    /// Returns true if pinky finger tip is closer to wrist than pinky knuckle joint.
    /// </summary>
    /// <param name="hand">Hand to query joint pose against.</param>
    protected bool IsRingGrabbing(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out var wristPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, hand, out var indexTipPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var indexKnucklePose))
        {
            // compare wrist-knuckle to wrist-tip
            Vector3 wristToIndexTip = indexTipPose.Position - wristPose.Position;
            Vector3 wristToIndexKnuckle = indexKnucklePose.Position - wristPose.Position;
            return wristToIndexKnuckle.sqrMagnitude >= wristToIndexTip.sqrMagnitude;
        }
        return false;
    }

    protected bool IsTwoHandsNear(Handedness hand_left, Handedness hand_right)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand_left, out var leftPalmPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand_right, out var rightPalmPose)
        )
        {
            Vector3 distance = leftPalmPose.Position - rightPalmPose.Position;
            return distance.sqrMagnitude <= 0.05;
        }
        return false;
    }

    protected bool IsThumbUp(Handedness hand)
    {
        bool is_thumb = !HandPoseUtils.IsThumbGrabbing(hand) &&
            HandPoseUtils.IsMiddleGrabbing(hand) &&
            HandPoseUtils.IsIndexGrabbing(hand) &&
            IsPinkyGrabbing(hand) &&
            IsRingGrabbing(hand);
        if (!is_thumb) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbMetacarpalJoint, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out var p2)
        )
        {
            Vector3 vec = p2.Position - p1.Position;
            Vector3 relative = new Vector3( 0, 1, 0 ); // up direction
            float angle = Vector3.Angle(vec, relative);
            return angle <= 45;
        }
        return false;
    }
}
