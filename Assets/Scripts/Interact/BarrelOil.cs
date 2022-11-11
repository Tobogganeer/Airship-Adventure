using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrelOil : MonoBehaviour
{
    public float fuelgrab;
    public Fuel classfuel;

    // Start is called before the first frame update
    void Start()
    {
        fuelgrab = classfuel.fuel;
        Debug.Log("Fuel : " + fuelgrab.ToString());
    }

    // Will need to take fuel value than -98% of it then make that the fill level
    // Update is called once per frame
    void Update()
    {
        
        
    }
}
