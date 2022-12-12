using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float speed = 5f;
    public float variance = 5f;
    public float intensity = 5f;
    float time;
    new Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        time = Random.Range(-1000f, 1000f);
    }

    void Update()
    {
        time += Time.deltaTime * speed;
        float noise = Mathf.PerlinNoise(time + 0.01f, time + 0.67f);
        light.intensity = intensity + (Mathf.Clamp01(noise) * 2f - 1f) * variance;
        //light.intensity = intensity.Evaluate(time);
    }
}
