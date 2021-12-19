using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;

/// <summary>
/// One hand gesture basic class
/// </summary>
public abstract class GestureWidget : Sensor
{
    static protected Vector3[] keyboardGrid = null;
    static protected float keyboardActiveTime;
    public Transform _target;
    public Camera _camera;
    public AudioSource _audioSource;
    public ToolTip _toolTip, _toolTip2;
    public Animator _animator;
    public Screen_Render _screen;
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
            // Debug.Log("DEBUG THUMB 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            // Debug.Log("DEBUG THUMB 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            // Debug.Log("DEBUG THUMB angle:" + angle.ToString());
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
            // Debug.Log("DEBUG INDEX 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            // Debug.Log("DEBUG INDEX 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            // Debug.Log("DEBUG INDEX angle:" + angle.ToString());
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
            // Debug.Log("DEBUG Middle 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            // Debug.Log("DEBUG Middle 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            // Debug.Log("DEBUG Middle angle:" + angle.ToString());
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
            // Debug.Log("DEBUG Ring 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            // Debug.Log("DEBUG Ring 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            // Debug.Log("DEBUG Ring angle:" + angle.ToString());
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
            // Debug.Log("DEBUG Pinky 1:" + Vector3.Angle(p2.Position - p1.Position, p3.Position - p1.Position).ToString());
            // Debug.Log("DEBUG Pinky 2:" + Vector3.Angle(p2.Position - p1.Position, p4.Position - p1.Position).ToString());
            // Debug.Log("DEBUG Pinky angle:" + angle.ToString());
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
            // Debug.Log("DEBUG SEVEN 1:"+(p2.Position-p1.Position).sqrMagnitude.ToString());
            // Debug.Log("DEBUG SEVEN 2:"+(p3.Position-p1.Position).sqrMagnitude.ToString());
            // Debug.Log("DEBUG SEVEN 3:"+(p3.Position-p2.Position).sqrMagnitude.ToString());
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
            // Debug.Log("DEBUG NINE INDEX 1:" + angle1.ToString());
            // Debug.Log("DEBUG NINE INDEX 2:" + angle2.ToString());
            // Debug.Log("DEBUG NINE INDEX angle:" + angle.ToString());
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
            return angle <= 30;
        }
        return false;
    }

    protected bool IsThumbDown(Handedness hand)
    {
        // if (!IsThumb(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbMetacarpalJoint, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out var p2)
        )
        {
            Vector3 vec = p2.Position - p1.Position;
            Vector3 relative = new Vector3( 0, -1, 0 ); // up direction
            float angle = Vector3.Angle(vec, relative);
            return angle <= 50;
        }
        return false;
    }

    protected bool IsThumbLeft(Handedness hand) {
        if (!IsThumb(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            Vector3 relative = new Vector3( 0, 1, 0 ); // up direction
            float angle = Vector3.Angle(norm, relative);
            return angle <= 40;
        }
        return false;
    }

    protected bool IsThumbRight(Handedness hand) {
        if (!IsThumb(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            Vector3 relative = new Vector3( 0, -1, 0 ); // up direction
            float angle = Vector3.Angle(norm, relative);
            return angle <= 50;
        }
        return false;
    }

    // =================================

    protected bool IsFiveClose(Handedness hand) {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out var p2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var p3) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, hand, out var p4) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, hand, out var p5)
        )
        {
            // Debug.Log("Five Close 1:  "+(p2.Position - p1.Position).sqrMagnitude.ToString());
            // Debug.Log("Five Close 2:  "+(p3.Position - p2.Position).sqrMagnitude.ToString());
            // Debug.Log("Five Close 3:  "+(p4.Position - p3.Position).sqrMagnitude.ToString());
            // Debug.Log("Five Close 4:  "+(p5.Position - p4.Position).sqrMagnitude.ToString());
            return (p2.Position-p1.Position).sqrMagnitude <= 0.002
                && (p3.Position-p2.Position).sqrMagnitude <= 0.002
                && (p4.Position-p3.Position).sqrMagnitude <= 0.002
                && (p5.Position-p4.Position).sqrMagnitude <= 0.005;
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

    protected bool IsTheForceUp(Handedness hand, Transform target)
    {
        if (!IsFive(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var p) && 
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            // Debug.Log("The Force hand:" + p.Position.ToString());
            // Debug.Log("The Force target:" + target.localPosition.ToString());
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            Vector3 vec = p.Position - target.localPosition;
            Vector3 relative = new Vector3(0, 1, 0); // up direction
            float angle_norm = Vector3.Angle(norm, relative);
            float angle_pos = Vector3.Angle(vec, relative);
            return angle_pos <= 30 && angle_norm >= 150;
        }
        return false;
    }

    protected bool IsTheForceDown(Handedness hand, Transform target)
    {
        if (!IsFive(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var p) && 
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, hand, out var q1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, hand, out var q2) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, hand, out var q3)
        )
        {
            // Debug.Log("The Force hand:" + p.Position.ToString());
            // Debug.Log("The Force target:" + target.localPosition.ToString());
            Vector3 norm = Vector3.Cross(q2.Position - q1.Position, q3.Position - q1.Position).normalized;
            Vector3 vec = p.Position - target.localPosition;
            Vector3 relative = new Vector3(0, 1, 0); // up direction
            float angle_norm = Vector3.Angle(norm, relative);
            float angle_pos = Vector3.Angle(vec, relative);
            return angle_pos <= 30 && angle_norm <= 30;
        }
        return false;
    }

    protected bool IsTouchObject(Handedness hand, Transform target)
    {
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var p)
        )
        {
            Vector3 vec = p.Position - target.position;
            // Debug.Log("hand Object: " + p.Position.ToString() + " touch Object: " + target.position.ToString() + " dis: "+vec.sqrMagnitude.ToString());
            return vec.sqrMagnitude <= 0.04;
        }
        else if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleMetacarpal, hand, out var p2)
        )
        {
            Vector3 vec = p2.Position - target.position;
            // Debug.Log("hand Object: " + p.Position.ToString() + " touch Object: " + target.position.ToString() + " dis: "+vec.sqrMagnitude.ToString());
            return vec.sqrMagnitude <= 0.04;
        }
        return false;
    }

    protected bool IsWaveRight(Handedness hand, Camera camera)
    {
        // if (!IsMiddleStraight(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var p2)
        )
        {
            // if(p1.Position.y < camera.transform.localPosition.y - 0.3) return false;
            Vector3 tmp = p2.Position - p1.Position;
            Debug.Log("Wave Right:  " + tmp);
            Vector3 vec_parallel = new Vector3(0, tmp.y, tmp.z);
            // Vector3 vec_vertical = new Vector3(tmp.x, tmp.y, 0);
            // Debug.Log("Wave Right:  " + vec.ToString());
            // Vector3 up = new Vector3( 0, 1, 0 ); // up direction
            Vector3 parallel = new Vector3( 0, 0, -1 );
            // float angle_up = Vector3.Angle(vec, up);
            float angle_parallel = Vector3.Angle(vec_parallel, parallel);
            // float angle_vertical = Vector3.Angle(vec_vertical, up);
            // Debug.Log("Wave Right angle_up:  " + angle_up.ToString());
            // Debug.Log("Wave Right angle_parallel:  " + angle_parallel.ToString());
            // return (angle_up >= 45 && angle_up <= 135) &&
            //         (angle_parallel <= 75);
            return angle_parallel <= 85;
        }
        return false;
    }

    protected bool IsWaveLeft(Handedness hand, Camera camera)
    {
        // if (!IsMiddleStraight(hand)) return false;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, hand, out var p1) &&
            HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, hand, out var p2)
        )
        {
            // if(p1.Position.y < camera.transform.localPosition.y - 0.3) return false;
            Vector3 tmp = p2.Position - p1.Position;
            Debug.Log("Wave Left:  " + tmp);
            Vector3 vec_parallel = new Vector3(0, tmp.y, tmp.z);
            // Vector3 vec_vertical = new Vector3(tmp.x, tmp.y, 0);
            // Debug.Log("Wave Left:  " + vec.ToString());
            // Vector3 up = new Vector3( 0, 1, 0 ); // up directSion
            Vector3 parallel = new Vector3( 0, 0, 1 );
            // float angle_up = Vector3.Angle(vec, up);
            float angle_parallel = Vector3.Angle(vec_parallel, parallel);
            // float angle_vertical = Vector3.Angle(vec_vertical, up);
            // Debug.Log("Wave Left angle_up:  " + angle_up.ToString());
            // Debug.Log("Wave Left angle_parallel:  " + angle_parallel.ToString());
            // return (angle_up >= 45 && angle_up <= 135) &&
            //         (angle_parallel <= 75);
            return angle_parallel <= 80;
        }
        return false;
    }
    
    // ================================================================

    public int keyboardGet(Handedness hand) {
        if (keyboardGrid == null) return -1;
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, hand, out var p)
        )
        {
            Vector3 pos = p.Position;
            pos = pos - keyboardGrid[0];
            float dx = keyboardGrid[1].sqrMagnitude;
            float dy = keyboardGrid[2].sqrMagnitude;
            float dz = keyboardGrid[3].sqrMagnitude;
            float x = Vector3.Dot(pos, keyboardGrid[1]) / dx;
            float y = Vector3.Dot(pos, keyboardGrid[2]) / dy;
            float z = Vector3.Dot(pos, keyboardGrid[3]) / dz;
            Debug.Log("z: " + z.ToString() + " dz: " + dz.ToString());
            // Debug.Log("Keyboard Check: expected: " + expected + " x: " + x + " y: " + y + " z: " + z);
            float scalex = 0.4f, scaley = 0.5f, scalez = 1f, pad = 0.2f;
            y += 0.5f * scaley;
            int predict = -1;
            if (y < -1.5 * scaley) return -1;
            else if (-1.5 * scaley < y && y < -0.6 * scaley && x > -0.5 * scalex) predict = 0;
            else {
                int px, py;
                if (x < -scalex - pad) px = 0;
                else if (x > scalex + pad) px = 2;
                else if (-scalex < x && x < scalex) px = 1;
                else return -1;
                if (y < scaley - pad) py = 2;
                else if (y > 3 * scaley + pad) py = 0;
                else if (scaley < y && y < 3 * scaley) py = 1;
                else return -1;

                predict = py * 3 + px + 1;
            }
            if ((predict == 5 && z < - 3 * scalez) || (predict != 5 && z < - scalez)) return -1;
            keyboardActiveTime = Time.time;
            return predict;
        }
        return -1;
    }


    

    protected void FillKeyboardGrid() {
        keyboardGrid = new Vector3[4];
        HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, _handedness_left, out var p0);
        HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, _handedness_left, out var p1);
        HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, _handedness_left, out var p2);
        HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, _handedness_left, out var p3);
        
        Vector3 cen = p0.Position;
        Vector3 hor = 2 * (p3.Position - p1.Position);
        Vector3 ver = p2.Position - p0.Position;
        Vector3 norm = Vector3.Cross(hor, ver).normalized * 0.005f;
        keyboardGrid[0] = cen;
        keyboardGrid[1] = hor;
        keyboardGrid[2] = ver;
        keyboardGrid[3] = norm;
        keyboardActiveTime = Time.time;
    }
}
