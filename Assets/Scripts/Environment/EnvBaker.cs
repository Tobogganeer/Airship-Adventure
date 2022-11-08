using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvBaker : MonoBehaviour
{
    public ReflectionProbe reflectionProbe;
    public RenderTexture target;
    int lastRender;

    private void Start()
    {
        lastRender = reflectionProbe.RenderProbe(target);
    }

    public void Bake()
    {
        if (reflectionProbe.IsFinishedRendering(lastRender))
            lastRender = reflectionProbe.RenderProbe(target);
    }
}
