using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    private float _scale_x, _scale_y, _scale_z; private bool is_scaling;
    private float _move_x, _move_y, _move_z; private bool is_moving;

    // Start is called before the first frame update
    void Start()
    {
        _scale_x = _scale_y = _scale_z = 0;
        _move_x = _move_x = _move_z = 0;
        is_moving = false;
        is_scaling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_scaling)
        {
            Vector3 delta = new Vector3(_scale_x, _scale_y, _scale_z);
            gameObject.transform.localScale = gameObject.transform.localScale + delta;
        }
        if (is_moving)
        {
            Vector3 delta = new Vector3(_move_x, _move_y, _move_z);
            gameObject.transform.localPosition = gameObject.transform.localPosition + delta;
        }
    }

    public void ScaleX(float x)
    {
        _scale_x = x;
        is_scaling = true;
    }

    public void ScaleY(float y)
    {
        _scale_y = y;
        is_scaling = true;
    }

    public void ScaleZ(float z)
    {
        _scale_z = z;
        is_scaling = true;
    }

    public void MoveX(float x)
    {
        _move_x = x;
        is_moving = true;
    }

    public void MoveY(float y)
    {
        _move_y = y;
        is_moving = true;
    }

    public void MoveZ(float z)
    {
        _move_z = z;
        is_moving = true;
    }

    public void CancelScale()
    {
        _scale_x = _scale_y = _scale_z = 0;
        is_scaling = false;
    }

    public void CancelMove()
    {
        _move_x = _move_x = _move_z = 0;
        is_moving = false;
    }
}
