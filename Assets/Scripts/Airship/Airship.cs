using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airship : MonoBehaviour
{
    public Vector3 movement;
    public Vector3 rot;

    void Update()
    {
        transform.Translate(movement * Time.deltaTime, Space.Self);
        transform.Rotate(rot * Time.deltaTime);
    }
}
