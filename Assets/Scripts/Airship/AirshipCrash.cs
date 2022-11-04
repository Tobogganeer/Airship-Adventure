using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCrash : MonoBehaviour
{
    [Layer]
    public int terrainLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == terrainLayer)
            Airship.Crash("Crashed into terrain!", 3f);
        // This object is set to 'Ship Crash' layer, which only collides with the 'Terrain' layer
        // ^^^ nvm lol
    }
}
