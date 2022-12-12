using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propellor : MonoBehaviour
{
    public float speed = 360;
    public float speedChangeAtMinAndMax = 180f;
    public float acceleration = 0.3f;
    float currentSpeed;

    public AudioSource audio1;
    public AudioSource audio2; // idk why there are 2 audios

    private void Start()
    {
        currentSpeed = speed;
    }

    void Update()
    {
        bool docked = DockingSystem.Docked || DockingSystem.Docking;
        float desired = docked ? 0 : speed + speedChangeAtMinAndMax * (Altitude.AirshipHeightFactor * 2f - 1);
        currentSpeed = Mathf.Lerp(currentSpeed, desired, Time.deltaTime * acceleration);

        transform.Rotate(Vector3.up * currentSpeed * Time.deltaTime, Space.Self);

        float pitch = desired / speed;
        float volume = Mathf.Clamp(desired / speed, 0, 1f);

        audio1.volume = volume;
        audio1.pitch = pitch;

        audio2.volume = volume;
        audio2.pitch = pitch;
    }
}
