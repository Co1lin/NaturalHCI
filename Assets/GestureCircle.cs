using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureCircle : GestureWidget
{
    private float prev_time;
    private int[] angle_backet = new int [18];

    public override bool GestureCondition()
    {
        if(Time.time - prev_time > 1f){
            prev_time = Time.time;
            for (int i = 0 ; i < 18 ; i++){
                angle_backet[i] = -1;
            }
        } 
        // if(CircleAngle(_handedness_right))
        // {
        int angle = CircleAngle(_handedness_right);
        angle_backet[angle / 20] = angle;
        // }
        for(int i = 0 ; i < 18 ; i++){
            if(angle_backet[i] == -1) return false;
        }
        return true;
    }
}