
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureLeftThumbDown : GestureWidget
{
    public override bool GestureCondition()
    {
        if (IsThumbDown(_handedness_left)) {
            _audioSource.volume -= 0.005f;
            Debug.Log("audio volume down: " + _audioSource.volume.ToString());
        }
        return false;
    }
}
