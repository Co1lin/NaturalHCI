using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System;

public class GestureKeyboardMultiple : GestureWidget
{
    int number = 0;
    int prev_number = -1;

    float prev_time = 0.0f;

    public override void Start()
    {
        base.Start();
        _audioSource.Pause();
    }

    public override bool GestureCondition()
    {
        int tmp = keyboardGet(_handedness_right);
        int prev = prev_number;
        if (prev != -1 && tmp == -1) {
            prev_time = Time.time;
            prev_number = -1;
            return true;
        }
        if (prev == -1 && tmp != -1 && Time.time - prev_time > 0.5f) {
            number = number * 10 + tmp;
            prev_number = tmp;
        }
        else if (keyboardGrid != null && Time.time - keyboardActiveTime > 2f) {
            number = Math.Max(Math.Min(number, 200), 30);
            _audioSource.pitch = number / 132.0f;
            number = 0;
            keyboardGrid = null;
        }
        _toolTip.ToolTipText = number.ToString();
        return false;
    }
}
