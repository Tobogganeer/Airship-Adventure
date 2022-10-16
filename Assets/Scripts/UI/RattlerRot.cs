using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattlerRot : MonoBehaviour
{
    public float speed = 0.1f;
    public float amplitude = 1f;
    public float period = 2f;

    [Space]
    public float scale = 0.1f;
    public float xMult = 2f;

    float time;
    Vector3 rot;
    Vector3 startRot;

    private void Start()
    {
        startRot = transform.eulerAngles;
    }

    void Update()
    {
        time += Time.deltaTime * speed;
        rot = Functions.Lissajous(time, amplitude, period);
        transform.rotation = Quaternion.Euler(startRot + rot.WithX(rot.x * xMult) * scale);
    }
}
