using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipObjects : MonoBehaviour
{
    public static ShipObjects instance;
    private void Awake()
    {
        instance = this;
    }


    public List<Transform> objects = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (!objects.Contains(other.transform) && other.HasTag("ShipObject"))
        {
            objects.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
