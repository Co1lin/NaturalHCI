using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano_Screen_Renderer : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material[] sheets;
    public int cur_page;
    public float activate_time;
    // Start is called before the first frame update
    void ChangeScreen(Material new_screen) {
        if (this.enabled) {
            meshRenderer.material = new_screen;
        }
    }
    public void ToPage(int num) {
        ChangeScreen(sheets[num]);
    }
    public void NextPage() {
        if(Time.time - activate_time < 1f)
            return;
        activate_time = Time.time;
        cur_page = (cur_page + 1) % sheets.Length;
        ToPage(cur_page);
    }

    public void PreviousPage() {
        if(Time.time - activate_time < 1f)
            return;
        activate_time = Time.time;
        cur_page = (cur_page - 1 + sheets.Length) % sheets.Length;
        ToPage(cur_page);
    }

    void Start()
    {
        cur_page = 0;
        activate_time = Time.time;
        ToPage(cur_page);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
