using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureWaveToLeft : GestureWidget
{
    private bool prev = false;
    private float prev_time;
    public override bool GestureCondition()
    {
        if(IsWaveRight(_handedness_right, _camera)){
            prev = true;
            prev_time = Time.time;
        } 
        else if(IsWaveLeft(_handedness_right, _camera))
        {
            if(prev && Time.time - prev_time < 1f)
            {
                prev_time = Time.time;
                return true;
            }
            prev = false;
        }
        return false;
    }
}