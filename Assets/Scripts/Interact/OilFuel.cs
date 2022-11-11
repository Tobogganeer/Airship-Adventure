using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilFuel : Fuel
{
    public Renderer oilLevelRenderer;
    public float levelAtMin = 0.4f;
    public float levelAtMax = 0.8f;

    Material mat;

    protected override void Start()
    {
        base.Start();
        mat = oilLevelRenderer.material;
        mat.SetFloat("_Fill", Remap.Float(fuel, minFuel, maxFuel, levelAtMin, levelAtMax));
    }

    private void OnDestroy()
    {
        DestroyImmediate(mat);
    }
}
