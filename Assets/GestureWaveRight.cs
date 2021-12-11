using Microsoft.MixedReality.Toolkit.Utilities;

public class GestureWaveToLeft : GestureWidget
{
    private bool prev = false;
    private float prev_time;
    public override bool GestureCondition()
    {
        if(IsWaveRight(_handedness_right)){
            prev = true;
            prev_time = Time.time;
        } 
        else if(IsWaveLeft(_handedness_right) )
        {
            if(prev && Time.time - prev_time < 1f)
            {
                prev = false;
                return true;
            }
            prev = false;
        }
        return false;
    }
}