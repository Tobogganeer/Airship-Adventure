using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public string sound;
    public float dist = 5f;

    [Space]
    public float minVelocity = 1f;
    public float minVelocityVolume = 0.2f;
    public float maxVolVelocity = 3f;
    public float maxVol = 1f;

    static float lastTime;
    const float MinTime = 0.05f;

    private void OnCollisionEnter(Collision collision)
    {
        float mag = collision.relativeVelocity.magnitude;
        if (mag < minVelocity || Time.time - lastTime < MinTime) return;

        float vol = Remap.Float(mag, minVelocity, Mathf.Min(mag, maxVolVelocity), minVelocityVolume, maxVol);
        AudioManager.Play(new Audio(sound).SetPosition(collision.contacts[0].point).SetDistance(dist).SetVolume(vol));
        lastTime = Time.time;
    }
}
