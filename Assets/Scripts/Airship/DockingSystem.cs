using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DockingSystem : MonoBehaviour
{
    public static bool Docking;
    public static bool Docked;
    public static bool RecentlyDocked;
    public static DockingSystem ActiveSystem;

    public float dockingDistance = 15f;
    public Transform releaseTo;
    bool InRange => Airship.instance != null ? transform.position.SqrDistance(
        Airship.instance.transform.position) < dockingDistance * dockingDistance : false;
    bool wasInRange;

    [Space]
    public Mesh debug_shipMesh;

    static bool tryDock;


    // ====================
    // This code is potentially the worst thing I have ever seen in my life
    // ====================



    private void Start()
    {
        Docked = false;
        Docking = false;
        ActiveSystem = null;
        RecentlyDocked = false;
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
                //HUD.SetDepartureIndicator(true);
                //HUD.SetDockIndicator(false);
                Airship.Docked = true;
                Airship.CanDock = false;
                Docking = false;
                Airship.Docking = false;

                //If i is pressed depart
                //if (Keyboard.current.iKey.wasPressedThisFrame)
                //{
                //    Docking = false;
                //    Airship.Docking = Docking;
                //    Docked = false;
                //    RecentlyDocked = true;
                //    Airship.Docked = false;
                //}
            }
            
            else if (Docking)
            {
                //HUD Jazz
                Airship.Docked = false;
                Airship.CanDock = false;

            }
            else
            {
                if (RecentlyDocked)
                {
                    //HUD Jazz
                    Airship.Docked = false;
                    Airship.CanDock = false;
                }
                else
                {
                    //HUD Jazz
                    Airship.Docked = false;
                    Airship.CanDock = true;
                }

                //If u is pressed dock
                //if (Keyboard.current.uKey.wasPressedThisFrame)
                if (tryDock)
                {
                    tryDock = false;
                    Airship.CanDock = false;
                    Docking = true;
                    Airship.Docking = Docking;
                    ActiveSystem = this;
                }
            }
        }
        else
        {
            if (wasInRange)
            {
                //HUD Jazz
                Airship.Docked = false;
                Airship.CanDock = false;
                Docked = false;
                RecentlyDocked = false;
                Docking = false;
                Airship.Docking = Docking;
            }
        }

        wasInRange = inRange;
        
    }

    public static void Release()
    {
        //Debug.Log("Try Release");
        if (Docked)
        {
            //Debug.Log("Release");
            Docking = false;
            Airship.Docking = Docking;
            Docked = false;
            RecentlyDocked = true;
            Airship.Docked = false;
        }
    }

    public static void Dock()
    {
        //Debug.Log("Try Dock");
        if (Airship.CanDock)
            tryDock = true;
        //DockingSystem active = ActiveSystem;
        //active?.DockInstance();
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
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireMesh(debug_shipMesh, transform.position, transform.rotation * Quaternion.Euler(0, 180, 0), Vector3.one * 0.5f);
            if (releaseTo)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireMesh(debug_shipMesh, releaseTo.position, releaseTo.rotation * Quaternion.Euler(0, 180, 0), Vector3.one * 0.5f);
            }
        }
    }
}
