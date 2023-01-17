using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingLever : MonoBehaviour, IInteractable
{
    public bool FixedPosition => false;
    public Transform InteractFrom => throw new System.NotImplementedException();
    public bool IsInteracting => false;

    public void OnInteract()
    {
        //throw new System.NotImplementedException();
        if (Airship.CanDock)
            DockingSystem.Dock();
        else if (Airship.Docked)
            DockingSystem.Release();
    }
}
