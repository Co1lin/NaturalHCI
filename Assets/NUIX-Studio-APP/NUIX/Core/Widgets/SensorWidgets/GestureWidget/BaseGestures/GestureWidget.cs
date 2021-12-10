using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using UnityEngine;

/// <summary>
/// One hand gesture basic class
/// </summary>
public abstract class GestureWidget : Sensor
{
    public Transform _target;
    protected Handedness _handedness_left, _handedness_right;

    private DateTime gestureStartTime;

    private bool countDownStarted { get; set; }

    /// <summary>
    /// Check the requirements, ex. which fingers should be grabbing
    /// </summary>
    /// <returns></returns>
    public abstract bool GestureCondition();

    public GestureWidget(Handedness left = Handedness.Left, Handedness right = Handedness.Right)
    {
        _handedness_left = left;
        _handedness_right = right;
    }
    
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
        // Debug.Log("DEBUG GLOBAL THUMB GRAB:"+IsThumbGrabbing(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL THUMB STRAIGHT:"+IsThumbStraight(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL INDEX GRAB:"+IsIndexGrabbing(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL INDEX STRAIGHT:"+IsIndexStraight(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL MIDDLE GRAB:"+IsMiddleGrabbing(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL MIDDLE STRAIGHT:"+IsMiddleStraight(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL RING GRAB:"+IsRingGrabbing(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL RING STRAIGHT:"+IsRingStraight(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL PINKY GRAB:"+IsPinkyGrabbing(_handedness_right).ToString());
        // Debug.Log("DEBUG GLOBAL PINKY STRAIGHT:"+IsPinkyStraight(_handedness_right).ToString());

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

    // ==========================

    protected bool IsThumbGrabbing(Handedness hand)
    {
        return HandPoseUtils.IsThumbGrabbing(hand);
    }

    protected bool IsIndexGrabbing(Handedness hand)
    {
        return HandPoseUtils.IsIndexGrabbing(hand);
    }

    protected bool IsMiddleGrabbing(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out var wristPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var tipPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleKnuckle, hand, out var knucklePose))
        {
            // compare wrist-knuckle to wrist-tip
            Vector3 wristToIndexTip = tipPose.Position - wristPose.Position;
            Vector3 wristToIndexKnuckle = knucklePose.Position - wristPose.Position;
            return wristToIndexKnuckle.sqrMagnitude + 0.005 >= wristToIndexTip.sqrMagnitude;
        }
        return false;
    }

    protected bool IsRingGrabbing(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out var wristPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, hand, out var tipPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var knucklePose))
        {
            // compare wrist-knuckle to wrist-tip
            Vector3 wristToIndexTip = tipPose.Position - wristPose.Position;
            Vector3 wristToIndexKnuckle = knucklePose.Position - wristPose.Position;
            return wristToIndexKnuckle.sqrMagnitude + 0.005 >= wristToIndexTip.sqrMagnitude;
        }
        return false;
    }
   
    protected bool IsPinkyGrabbing(Handedness hand)
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out var wristPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, hand, out var tipPose) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, hand, out var knucklePose))
        {
            // compare wrist-knuckle to wrist-tip
            Vector3 wristToIndexTip = tipPose.Position - wristPose.Position;
            Vector3 wristToIndexKnuckle = knucklePose.Position - wristPose.Position;
            return wristToIndexKnuckle.sqrMagnitude + 0.005 >= wristToIndexTip.sqrMagnitude;
        }
        return false;
    }

    // =========================

    protected bool IsThumbStraight(Handedness hand)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbMetacarpalJoint, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbProximalJoint, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbDistalJoint, hand, out var p3) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out var p4) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            float angle = Mathf.Abs(Vector3.SignedAngle(p4.Position - p1.Position, norm, q2.Position - q1.Position));
            Debug.Log("DEBUG THUMB 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            Debug.Log("DEBUG THUMB 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            Debug.Log("DEBUG THUMB angle:" + angle.ToString());
            return Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position) <= 20
                && Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position) <= 20
                && 70 <= angle && angle <= 110;
        }
        return false;
    }

    protected bool IsIndexStraight(Handedness hand)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexMiddleJoint, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexDistalJoint, hand, out var p3) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out var p4) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            float angle = Mathf.Abs(Vector3.SignedAngle(p4.Position - p1.Position, norm, q2.Position - q1.Position));
            Debug.Log("DEBUG INDEX 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            Debug.Log("DEBUG INDEX 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            Debug.Log("DEBUG INDEX angle:" + angle.ToString());
            return Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position) <= 20
                && Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position) <= 20
                && 60 <= angle && angle <= 120;
        }
        return false;
    }

    protected bool IsMiddleStraight(Handedness hand)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleKnuckle, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleMiddleJoint, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleDistalJoint, hand, out var p3) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var p4) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            float angle = Mathf.Abs(Vector3.SignedAngle(p4.Position - p1.Position, norm, q2.Position - q1.Position));
            Debug.Log("DEBUG Middle 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            Debug.Log("DEBUG Middle 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            Debug.Log("DEBUG Middle angle:" + angle.ToString());
            return Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position) <= 20
                && Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position) <= 20
                && 60 <= angle && angle <= 120;
        }
        return false;
    }

    protected bool IsRingStraight(Handedness hand)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingMiddleJoint, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingDistalJoint, hand, out var p3) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, hand, out var p4) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            float angle = Mathf.Abs(Vector3.SignedAngle(p4.Position - p1.Position, norm, q2.Position - q1.Position));
            Debug.Log("DEBUG Ring 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            Debug.Log("DEBUG Ring 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            Debug.Log("DEBUG Ring angle:" + angle.ToString());
            return Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position) <= 20
                //&& Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position) <= 20
                && 60 <= angle && angle <= 120;
        }
        return false;
    }

    protected bool IsPinkyStraight(Handedness hand)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyMiddleJoint, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyDistalJoint, hand, out var p3) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, hand, out var p4) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            float angle = Mathf.Abs(Vector3.SignedAngle(p4.Position - p1.Position, norm, q2.Position - q1.Position));
            Debug.Log("DEBUG Pinky 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            Debug.Log("DEBUG Pinky 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            Debug.Log("DEBUG Pinky angle:" + angle.ToString());
            return Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position) <= 20
                && Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position) <= 20
                && 60 <= angle && angle <= 120;
        }
        return false;
    }

    // =================================

    protected bool IsOne(Handedness hand)
    {
        return IsThumbGrabbing(hand)
            && IsIndexStraight(hand)
            && IsMiddleGrabbing(hand)
            && IsRingGrabbing(hand)
            && IsPinkyGrabbing(hand);
    }

    protected bool IsTwo(Handedness hand)
    {
        return IsThumbGrabbing(hand)
            && IsIndexStraight(hand)
            && IsMiddleStraight(hand)
            && IsRingGrabbing(hand)
            && IsPinkyGrabbing(hand);
    }

    protected bool IsThree(Handedness hand)
    {
        return IsThumbGrabbing(hand)
            && IsIndexStraight(hand)
            && IsMiddleStraight(hand)
            && IsRingStraight(hand)
            && IsPinkyGrabbing(hand);
    }

    protected bool IsFour(Handedness hand)
    {
        return IsThumbGrabbing(hand)
            && IsIndexStraight(hand)
            && IsMiddleStraight(hand)
            && IsRingStraight(hand)
            && IsPinkyStraight(hand);
    }

    protected bool IsFive(Handedness hand)
    {
        return IsThumbStraight(hand)
            && IsIndexStraight(hand)
            && IsMiddleStraight(hand)
            && IsRingStraight(hand)
            && IsPinkyStraight(hand);
    }

    protected bool IsSix(Handedness hand)
    {
        return IsThumbStraight(hand)
            && IsIndexGrabbing(hand)
            && IsMiddleGrabbing(hand)
            && IsRingGrabbing(hand)
            && IsPinkyStraight(hand);
    }

    protected bool IsSeven(Handedness hand)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var p3)
        )
        {
            Debug.Log("FUCK 1:"+(p2.Position-p1.Position).sqrMagnitude.ToString());
            Debug.Log("FUCK 2:"+(p3.Position-p1.Position).sqrMagnitude.ToString());
            Debug.Log("FUCK 3:"+(p3.Position-p2.Position).sqrMagnitude.ToString());
            return (p2.Position-p1.Position).sqrMagnitude <= 0.0008
                && (p3.Position-p1.Position).sqrMagnitude <= 0.0008
                && !IsThumbGrabbing(hand) && !IsIndexGrabbing(hand) && !IsMiddleGrabbing(hand);
        }
        return false;
    }

    protected bool IsEight(Handedness hand)
    {
        return IsThumbStraight(hand)
            && IsIndexStraight(hand)
            && IsMiddleGrabbing(hand)
            && IsRingGrabbing(hand)
            && IsPinkyGrabbing(hand);
    }

    protected bool IsNine(Handedness hand)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexMiddleJoint, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexDistalJoint, hand, out var p3) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out var p4) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            float angle = Mathf.Abs(Vector3.SignedAngle(p2.Position - p1.Position, norm, q2.Position - q1.Position));
            float angle1 = Vector3.Angle(p2.Position - p1.Position, p3.Position - p2.Position);
            float angle2 = Vector3.Angle(p3.Position - p2.Position, p4.Position - p3.Position);
            Debug.Log("DEBUG NINE INDEX 1:" + angle1.ToString());
            Debug.Log("DEBUG NINE INDEX 2:" + angle2.ToString());
            Debug.Log("DEBUG NINE INDEX angle:" + angle.ToString());
            return 50 <= angle1 && angle1 <= 130
                && 50 <= angle2 && angle2 <= 130
                && 60 <= angle && angle <= 120;
        }
        return false;
    }

    protected bool IsZero(Handedness hand)
    {
        return IsThumbGrabbing(hand)
            && IsIndexGrabbing(hand)
            && IsMiddleGrabbing(hand)
            && IsRingGrabbing(hand)
            && IsPinkyGrabbing(hand);
    }

    // =================================

    protected bool IsThumb(Handedness hand)
    {
        return IsThumbStraight(hand) &&
            IsMiddleGrabbing(hand) &&
            IsIndexGrabbing(hand) &&
            IsPinkyGrabbing(hand) &&
            IsRingGrabbing(hand);
    }

    protected bool IsThumbUp(Handedness hand)
    {
        if (!IsThumb(hand)) return false;
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

    protected bool IsTheForce(Handedness hand, Transform target)
    {
        if (!IsFive(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var p)
        )
        {
            Vector3 vec = p.Position - target.localPosition;
            Vector3 relative = new Vector3(0, 1, 0); // up direction
            float angle = Vector3.Angle(vec, relative);
            return angle <= 30;
        }
        return false;
    }

    protected bool IsTouchObject(Handedness hand, Transform target)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var p)
        )
        {
            Vector3 vec = p.Position - target.localPosition;
            return vec.sqrMagnitude <= 0.1;
        }
        else if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleMetacarpal, hand, out var p2)
        )
        {
            Vector3 vec = p2.Position - target.localPosition;
            return vec.sqrMagnitude <= 0.1;
        }
        return false;
    }
}
