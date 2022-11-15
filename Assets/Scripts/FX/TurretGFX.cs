using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGFX : MonoBehaviour
{
    public Transform lowerHydraulic;
    public Transform upperHydraulic;
    public Transform rotateGear;
    public Transform winchGear;
    public Transform shaft;
    public Transform head;

    [Space]
    public Vector3 gearOffset;
    public Vector3 gearFactor;


    private void LateUpdate()
    {
        //rotateGear.localRotation = Quaternion.Euler(head.eulerAngles.);
        Gear();
        Hydraulics();
    }

    void Gear()
    {
        // Convert from x rot to z rot
        Vector3 headRot = head.localEulerAngles;
        headRot = gearFactor * headRot.x + gearOffset;
        rotateGear.localRotation = Quaternion.Euler(headRot);
    }

    void Hydraulics()
    {
        lowerHydraulic.LookAt(upperHydraulic);
        upperHydraulic.LookAt(lowerHydraulic);
    }
}
