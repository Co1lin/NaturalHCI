using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Screen_Render : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material[] channels;
    public Material screen_saver;
    int count;
    bool brightness_uping;
    bool brightness_downing;
    void ChangeScreen(Material new_screen) {
        Color emission = meshRenderer.material.GetColor("_EmissionColor");
        meshRenderer.material = new_screen;
        meshRenderer.material.SetColor("_EmissionColor", emission);
    }
    public void ToChannel(int num) {
        ChangeScreen(channels[num - 1]);
    }

    public void ToScreenSaver() {
        ChangeScreen(screen_saver);
    }
    public void BrightnessUp() {
        brightness_uping = true;
    }
    public void BrightnessDown() {
        brightness_downing = true;
    }

    public void CancelBrightness() {
        brightness_uping = brightness_downing = false;
    }

    void Update() {
        count += 1;
        if(count > 5) {
            count = 0;
            if(brightness_uping) {
                Color emission = meshRenderer.material.GetColor("_EmissionColor");
                if(emission.r < 0.7) {
                    emission.r = emission.g = emission.b = emission.r + 0.015f;
                    meshRenderer.material.SetColor("_EmissionColor", emission);
                }
            }
            if(brightness_downing) {
                Color emission = meshRenderer.material.GetColor("_EmissionColor");
                if(emission.r > 0.05) {
                    emission.r = emission.g = emission.b = emission.r - 0.015f;
                    meshRenderer.material.SetColor("_EmissionColor", emission);
                }
            }
        }
    }
    void Start() {   
        count = 0;
        brightness_uping = false;
        brightness_downing = false;
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
