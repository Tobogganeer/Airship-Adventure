using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaTesting : MonoBehaviour
{
    public Transform target;

    public float delta;

    void Update()
    {
        float y = target.eulerAngles.y;
        float deltaAngle = y - transform.eulerAngles.y;
        //if (deltaAngle < -360) deltaAngle += 360;
        //if (deltaAngle > 360) deltaAngle -= 360;
        if (deltaAngle > 180) deltaAngle -= 360;
        delta = deltaAngle;
    }
}
