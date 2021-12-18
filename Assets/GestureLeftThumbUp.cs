
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureLeftThumbUp : GestureWidget
{
    public override bool GestureCondition()
    {
        if (IsThumbUp(_handedness_left)) {
            _audioSource.volume += 0.005f;
            // Debug.Log("audio volume up: " + _audioSource.volume.ToString());
        }
        return false;
    }
}
