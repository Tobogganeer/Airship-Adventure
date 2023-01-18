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

    public Vector2 bgWindTime = new Vector2(60, 120);
    public AudioSource bgSource;
    float time;


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

    private void Start()
    {
        time = Random.Range(bgWindTime.x, bgWindTime.y);
    }

    private void Update()
    {
        source.volume = Mathf.MoveTowards(source.volume, target, Time.deltaTime * Speed * baseVolume);

        time -= Time.deltaTime;

        if (time <= 0)
        {
            time = Random.Range(bgWindTime.x, bgWindTime.y);
            bgSource.pitch = Random.Range(0.75f, 1.3f);
            bgSource.Play();
            //AudioManager.Play(new Audio("BackgroundWind").SetGlobal().SetPitch(0.75f, 1.3f));
        }
    }
}
