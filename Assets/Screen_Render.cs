using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Screen_Render : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material[] channels;
    public Material screenSaver;
    public void To_Channel(int num)
    {
        meshRenderer.material = channels[num - 1];      
    }

    void Load_Sources(){
        screenSaver = Resources.Load<Material>("Materials/ScreenSaver");
        channels[0] = Resources.Load<Material>("Materials/CCTV1");
    }
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Load_Sources();
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         //Output the amount of materials before GameObject is destroyed
    //         print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    //         //Destroy GameObject
    //         Destroy(gameObject);
    //     }
    // }

    // void OnMouseOver()
    // {
    //     // Change the Color of the GameObject when the mouse hovers over it
    //     m_Material.color = Color.red;
    // }

    // void OnMouseExit()
    // {
    //     //Change the Color back to white when the mouse exits the GameObject
    //     m_Material.color = Color.white;
    // }

    // void OnDestroy()
    // {
    //     //Destroy the instance
    //     Destroy(m_Material);
    //     //Output the amount of materials to show if the instance was deleted
    //     print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    // }
}
