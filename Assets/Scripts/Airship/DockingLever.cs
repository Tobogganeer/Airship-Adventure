using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingLever : MonoBehaviour
{
    public GameObject indicator;

    private void Update()
    {
        indicator.SetActive(Airship.CanDock || Airship.Docked);
    }

    public void OnInteract()
    {
        if (Airship.CanDock)
            DockingSystem.Dock();
        else if (Airship.Docked)
            DockingSystem.Release();
    }
}
