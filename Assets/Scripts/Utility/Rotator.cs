using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotation;

    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }
}
