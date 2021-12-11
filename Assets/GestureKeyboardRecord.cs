using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GestureKeyboardRecord : GestureWidget
{
    public override bool GestureCondition()
    {
        if (keyboardGrid == null && IsFive(_handedness_left)) {
            Debug.Log("Grid Record: ");
            FillKeyboardGrid();
            Debug.Log("Grid Record: "+keyboardGrid[0].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[1].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[2].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[3].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[4].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[5].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[6].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[7].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[8].ToString());
            Debug.Log("Grid Record: "+keyboardGrid[9].ToString());
        }
        else if (keyboardGrid != null && Time.time - keyboardActiveTime > 4f) {
            keyboardGrid = null;
        }
        return false;
    }
}
