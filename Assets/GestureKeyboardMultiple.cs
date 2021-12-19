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
        if (!_audioSource.enabled) {
            number = 0;
            keyboardGrid = null;
            return false;
        }
        if (IsFive(_handedness_left)) FillKeyboardGrid();
        int tmp = keyboardGet(_handedness_right);
        int prev = prev_number;
        if (prev != -1 && tmp == -1) {
            prev_time = Time.time;
            prev_number = -1;
            return true;
        }
        if (prev == -1 && tmp != -1 && Time.time - prev_time > 0.8f) {
            number = number * 10 + tmp;
            prev_number = tmp;
            _toolTip.ToolTipText = number.ToString();
        }
        if (keyboardGrid != null && Time.time - keyboardActiveTime > 2.0f) {
            Debug.Log("KeyKey Multiple: " + number.ToString() + " audio enabled: " + _audioSource.enabled.ToString());
            number = Math.Max(Math.Min(number, 200), 30);
            _audioSource.pitch = number / 132.0f;
            _toolTip.ToolTipText = number.ToString() + " beats";
            number = 0;
            keyboardGrid = null;
        }
        return false;
    }
}
