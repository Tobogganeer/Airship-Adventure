using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DockingSysten : MonoBehaviour
{
    public Boolean Docking;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Docking != true)
            {
                PopUp.Show("Press U To Dock Ship");
                        
                if (Keyboard.current.uKey.wasPressedThisFrame)
                {
                   Docking = true;
                }
                
            }
            else
            {
                PopUp.Show("Docking");
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Turn off Hud
        }
    }

}
