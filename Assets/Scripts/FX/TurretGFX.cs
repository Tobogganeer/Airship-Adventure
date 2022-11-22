using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGFX : MonoBehaviour
{
    public GrappleHook hook;

    [Space]
    public Transform lowerHydraulic;
    public Transform upperHydraulic;
    public Transform rotateGear;
    public Transform winchGear;
    public Transform shaft;
    public Transform head;
    public float winchSpeed = 100f;

    float winch;
    float winchSpeedSmooth;

    [Space]
    public Vector3 gearOffset;
    public Vector3 gearFactor;


    private void LateUpdate()
    {
        //rotateGear.localRotation = Quaternion.Euler(head.eulerAngles.);
        Gear();
        Hydraulics();
        Winch();
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

    void Winch()
    {
        if (hook.grabbing && !hook.targetOnHook)
            winchSpeedSmooth = Mathf.Lerp(winchSpeedSmooth, winchSpeed, Time.deltaTime * 5);
        //winch += Time.deltaTime * winchSpeed;
        else
            winchSpeedSmooth = Mathf.Lerp(winchSpeedSmooth, -winchSpeed, Time.deltaTime * 5);
        winch += Time.deltaTime * winchSpeedSmooth;
        winch = Mathf.Max(winch, 0);
        winchGear.localRotation = Quaternion.Euler(-winch, 0, -90);
    }
}
