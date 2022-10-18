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

    public bool HUDVisible = true;
}
