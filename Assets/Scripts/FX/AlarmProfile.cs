using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Alarm Profile")]
public class AlarmProfile : ScriptableObject
{
    public AudioClip clip;
    public float audioPeriod = 1f;
    public float volume = 1f;

    float time;
    AudioSource audioSource;

    public void Tick(float dt)
    {
        time += dt;
        if (time > audioPeriod)
        {
            time = 0;
            audioSource.Play();
        }
    }

    public void Reset(AudioSource alarmSource)
    {
        time = 0;
        audioSource = alarmSource;
        audioSource.clip = clip;
        audioSource.volume = volume;
    }
}
