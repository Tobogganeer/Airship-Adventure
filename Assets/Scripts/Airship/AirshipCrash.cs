using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCrash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Airship.Crash("Crashed into terrain!", 3f);
        // This object is set to 'Ship Crash' layer, which only collides with the 'Terrain' layer
    }
}
