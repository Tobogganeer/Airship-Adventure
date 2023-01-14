using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    static Music instance;
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        baseVolume = source.volume;
        target = baseVolume;
    }

    AudioSource source;
    float target;
    float baseVolume;
    const float Speed = 0.5f;


    public static void Stop()
    {
        instance.target = 0f;
    }

    public static void StopImmediately()
    {
        instance.target = 0f;
        instance.source.volume = 0f;

    }

    public static void Play()
    {
        instance.target = instance.baseVolume;
    }

    private void Update()
    {
        source.volume = Mathf.MoveTowards(source.volume, target, Time.deltaTime * Speed * baseVolume);
    }
}
