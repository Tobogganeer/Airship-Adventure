using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    public float minFuel;
    public float maxFuel;

    [Space, ReadOnly]
    public float fuel;

    private void Start()
    {
        fuel = Random.Range(minFuel, maxFuel);
    }
}