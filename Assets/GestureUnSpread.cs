using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System.Collections;

public class GestureUnSpread : GestureWidget
{
    private bool prev = false;
    private float prev_time;
    public override bool GestureCondition()
    {
        if (IsFive(_handedness_right)) {
            prev = true;
            prev_time = Time.time;
        }
        else if (IsFiveClose(_handedness_right)) {
            if (prev && Time.time - prev_time < 4f) {
                prev_time = Time.time;
                return true;
            }
            prev = false;
        }
        return false;
    }
}