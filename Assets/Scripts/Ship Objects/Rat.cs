using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    public float maxDist = 2f;

    void Update()
    {
        if (transform.localPosition.magnitude > maxDist)
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
