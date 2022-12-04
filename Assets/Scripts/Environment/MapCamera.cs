using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public float updateRate = 1.0f;
    float time;
    Camera cam;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            time = updateRate;
            cam.Render();
        }
    }
}
