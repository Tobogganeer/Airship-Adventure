using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private static HUD instance;
    private void Awake()
    {
        instance = this;
    }

    public CanvasGroup interactIcon;
    bool interact;


    //public static bool HUDVisible = true;

    public static void SetInteract(bool on)
    {
        instance.interact = on;
    }



    private void Update()
    {
        interactIcon.alpha = Mathf.Lerp(interactIcon.alpha, interact ? 1 : 0, Time.deltaTime * 10);
    }
}
