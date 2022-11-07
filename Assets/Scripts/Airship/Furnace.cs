using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Fuel fuel))
        {
            Airship.Fuel += fuel.fuel;
            Destroy(other.gameObject);
        }
    }
}
