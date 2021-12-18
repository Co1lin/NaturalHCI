using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureKeyboardRecord : GestureWidget
{
    public override bool GestureCondition()
    {
        if (IsFive(_handedness_left)) {
            FillKeyboardGrid();
            return true;
        }
        return false;
    }
}
