using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propellor : MonoBehaviour
{
    public float speed = 360;
    public float speedChangeAtMinAndMax = 180f;
    public float acceleration = 0.3f;
    float currentSpeed;
    public float volumeMult = 1f;
    public Vector3 axis = Vector3.right;

    public AudioSource audio1;
    public AudioSource audio2; // idk why there are 2 audios

    private void Start()
    {
        currentSpeed = speed;
    }

    void Update()
    {
        bool docked = DockingSystem.Docked || DockingSystem.Docking;
        float nox = Airship.Nox > 0 ? Airship.instance.nitrousSpeedMult : 1f;
        float desired = (docked || Airship.Fuel <= 0) ? 0 : speed + speedChangeAtMinAndMax * (Altitude.AirshipHeightFactor * 2f - 1);
        desired *= nox;
        currentSpeed = Mathf.Lerp(currentSpeed, desired, Time.deltaTime * acceleration);

        transform.Rotate(axis * currentSpeed * Time.deltaTime, Space.Self);

        float pitch = desired / speed;
        float volume = Mathf.Clamp(desired / speed, 0f, 1f);

        audio1.volume = volume * volumeMult;
        audio1.pitch = pitch;

        audio2.volume = volume * volumeMult;
        audio2.pitch = pitch;
    }
}
