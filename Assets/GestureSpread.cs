using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System.Collections;

public class GestureSpread : GestureWidget
{
    private bool prev = false;
    private float prev_time;
    public override bool GestureCondition()
    {
        if (IsFiveClose(_handedness_right)) {
            prev = true;
            prev_time = Time.time;
            Debug.Log("prev_time 1: " + prev_time.ToString());
        }
        else if (IsFive(_handedness_right)) {
            if (prev && Time.time - prev_time < 4f) {
                prev_time = Time.time;
                return true;
            }
            prev = false;
        }
        return false;
    }
}