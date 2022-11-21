using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DockingSystem : MonoBehaviour
{
    public static bool Docking;
    public static DockingSystem ActiveSystem;

    public float dockingDistance = 15f;
    bool InRange => Airship.instance != null ? transform.position.SqrDistance(
        Airship.instance.transform.position) < dockingDistance * dockingDistance : false;
    bool wasInRange;

    private void Update()
    {
        bool inRange = InRange;

        if (inRange)
        {
            if (Docking)
            {
                HUD.SetDockIndicator(false);
            }
            else
            {
                HUD.SetDockIndicator(true);

                if (Keyboard.current.uKey.wasPressedThisFrame)
                {
                    Docking = true;
                    ActiveSystem = this;
                }
            }
        }
        else
        {
            if (wasInRange)
            {
                HUD.SetDockIndicator(false);
            }
        }

        wasInRange = inRange;
    }

    /*
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!Docking)
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
    */

    private void OnDrawGizmos()
    {
        if (InRange)
            Gizmos.color = Color.yellow;
        else
            Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, dockingDistance);
    }

    private void OnDisable()
    {
        HUD.SetDockIndicator(false);
    }
}
