using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingChute : MonoBehaviour
{
    public MerchantDock dock;

    // If you want an explanation @ me lol
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);

        if (other.TryGetComponent(out Crate crate))
        {
            dock.AddValueToCart(crate.value);
            Destroy(other.gameObject);
            // If the other object is fuel, add fuel to the ship and destroy it
        }
    }
}
