using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotation;

    void Update()
    {

        if (DockingSystem.Docked)
        {

        }
        else
        {
            transform.Rotate(rotation * Time.deltaTime, Space.Self);
        }
        
    }
}
