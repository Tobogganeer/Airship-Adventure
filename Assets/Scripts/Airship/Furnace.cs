using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    // If you want an explanation @ me lol
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Fuel fuel))
        {
            Airship.Fuel += fuel.fuel;
            Destroy(other.gameObject);
            // If the other object is fuel, add fuel to the ship and destroy it
        }
    }
}
