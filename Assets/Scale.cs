using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    private float _y;
    private bool is_changing;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (is_changing)
        {
            Vector3 delta = new Vector3(0, _y, 0);
            gameObject.transform.localScale = gameObject.transform.localScale + delta;
        }
    }

    public void HeightScale(float y)
    {
        _y = y;
        is_changing = true;
    }

    public void CancelScale()
    {
        is_changing = false;
    }
}
