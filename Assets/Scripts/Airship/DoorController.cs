using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public GameObject doorCollider;

    void Update()
    {
        door.SetActive(!Airship.Docked);
        doorCollider.SetActive(!Airship.Docked);
    }
}
