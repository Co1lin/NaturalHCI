using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureKeyboardRecord : GestureWidget
{
    public override bool GestureCondition()
    {
        if (/*keyboardGrid == null && */IsFive(_handedness_left)) {
            FillKeyboardGrid();
            return true;
        }
        else if (keyboardGrid != null && Time.time - keyboardActiveTime > 4f) {
            keyboardGrid = null;
        }
        return false;
    }
}
