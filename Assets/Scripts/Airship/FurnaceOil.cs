using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceOil : MonoBehaviour
{
    public Renderer oilLevelRenderer;
    public float levelAtMin = 0.4f;
    public float levelAtMax = 0.8f;

    Material mat;
    float fill;

    void Start()
    {
        mat = oilLevelRenderer.material;
        //mat.SetFloat("_Fill", Remap.Float(fuel, minFuel, maxFuel, levelAtMin, levelAtMax));
    }

    private void Update()
    {
        fill = Mathf.Lerp(fill, Airship.Fuel01, Time.deltaTime * 5f);
        mat.SetFloat("_Fill", Remap.Float(fill, 0, 1, levelAtMin, levelAtMax));
    }

    private void OnDestroy()
    {
        DestroyImmediate(mat);
    }
}
