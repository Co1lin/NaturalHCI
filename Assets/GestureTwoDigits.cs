using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;

public class GestureTwoDigits : GestureWidget
{
    public override bool GestureCondition()
    {
        int number_left = getNumber(_handedness_left);
        int number_right = getNumber(_handedness_right);
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, _handedness_left, out var palm)) {
            if (number_left != -1 && number_right != -1) {
                int number = number_left * 10 + number_right;
                work(number);
            }
        } else if (number_right != -1) {
            int number = number_right;
            work(number);
        }
        return false;
    }

    private void work(int number) {
        Debug.Log("Two Digits: " + number.ToString() + " enabled: " + _screen.enabled.ToString());
        if (_screen.enabled) {
            _screen.ToChannel(number);
            _toolTip.ToolTipText = number.ToString();
        }
    }

    private int getNumber(Handedness hand) {
        if (IsOne(hand)) return 1;
        if (IsTwo(hand)) return 2;
        if (IsThree(hand)) return 3;
        if (IsFour(hand)) return 4;
        if (IsFive(hand)) return 5;
        if (IsSix(hand)) return 6;
        if (IsSeven(hand)) return 7;
        if (IsEight(hand)) return 8;
        if (IsNine(hand)) return 9;
        if (IsZero(hand)) return 0;
        return -1;
    }
}