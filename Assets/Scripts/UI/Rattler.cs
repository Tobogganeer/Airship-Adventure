using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rattler : MonoBehaviour
{
    public float speed = 0.2f;
    public float amplitude = 1f;
    public float period = 2f;

    [Space]
    public float scale = 0.7f;
    public float scaleY = 1.5f;
    public float lerpSpeed = 2f;

    float time1;
    float time2;
    float time3;
    Vector3 pos;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        time1 += Time.deltaTime * speed;
        time2 += Time.deltaTime * speed * 0.8f;
        time2 += Time.deltaTime * speed * 0.5f;
        pos = Functions.Lissajous3D(time1, time2, time3, amplitude, period);
        transform.position = Vector3.Lerp(transform.position, startPos + pos.WithY(pos.y * scaleY) * scale, Time.deltaTime * lerpSpeed);
    }
}
