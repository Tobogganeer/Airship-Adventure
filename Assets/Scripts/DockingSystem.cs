using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DockingSystem : MonoBehaviour
{
    public static bool Docking;
    public static bool Docked;
    public static bool recentlyDocked;
    public static DockingSystem ActiveSystem;

    public float dockingDistance = 15f;
    bool InRange => Airship.instance != null ? transform.position.SqrDistance(
        Airship.instance.transform.position) < dockingDistance * dockingDistance : false;
    bool wasInRange;

    [Space]
    public Mesh debug_shipMesh;


    private void Start()
    {
        Docked = false;
        Docking = false;
        ActiveSystem = null;
        recentlyDocked = false;
    }


    private void Update()
    {
        bool inRange = InRange;

        if (inRange)
        {
            //Check To see if Ship is Docked
            if (Docked)
            {
                //HUD Jazz
                HUD.SetDepartureIndicator(true);
                HUD.SetDockIndicator(false);

                //If i is pressed depart
                if (Keyboard.current.iKey.wasPressedThisFrame)
                {
                    Docking = false;
                    Docked = false;
                    recentlyDocked = true;
                    HUD.SetDepartureIndicator(false);
                }
            }
            
            else if (Docking)
            {
                //HUD Jazz
                HUD.SetDockIndicator(false);
                HUD.SetDepartureIndicator(false);

            }
            else
            {
                if (recentlyDocked)
                {
                    //HUD Jazz
                    HUD.SetDockIndicator(false);
                    HUD.SetDepartureIndicator(false);
                }
                else
                {
                    //HUD Jazz
                    HUD.SetDockIndicator(true);
                    HUD.SetDepartureIndicator(false);
                }

                //If u is pressed dock
                if (Keyboard.current.uKey.wasPressedThisFrame)
                {
                    HUD.SetDockIndicator(false);
                    Docking = true;
                    ActiveSystem = this;
                }
            }
        }
        else
        {
            if (wasInRange)
            {
                //HUD Jazz
                HUD.SetDockIndicator(false);
                HUD.SetDepartureIndicator(false);
                Docked = false;
                recentlyDocked = false;
                Docking = false;
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

    private void OnDrawGizmosSelected()
    {
        if (debug_shipMesh)
            Gizmos.DrawWireMesh(debug_shipMesh, transform.position, transform.rotation, Vector3.one * 0.5f);
    }

    private void OnDisable()
    {
        //HUD Jazz
        HUD.SetDockIndicator(false);
        HUD.SetDepartureIndicator(false);
    }
}
