using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float mult = 4f;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, mult * Airship.Turn);
    }
}
