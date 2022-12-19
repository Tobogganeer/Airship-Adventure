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

    [ContextMenu("Bake")]
    public void Bake()
    {
        transform.rotation = Quaternion.identity;
        if (reflectionProbe.IsFinishedRendering(lastRender))
            lastRender = reflectionProbe.RenderProbe(target);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
