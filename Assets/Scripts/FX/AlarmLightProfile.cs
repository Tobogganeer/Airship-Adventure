using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Alarm Light Profile")]
public class AlarmLightProfile : ScriptableObject
{
    public float period = 1f;
    [ColorUsage(false, true)] public Color colourHigh;
    [ColorUsage(false, true)] public Color colourLow;
    public AnimationCurve brightnessCurve;

    float time;
    Material mat;

    public void Tick(float dt)
    {
        time += dt;
        if (time > period)
        {
            time = 0;
        }

        mat.SetColor("_EmissionColor", Color.Lerp(colourLow, colourHigh, brightnessCurve.Evaluate(Remap.Float01(time, 0, period))));
    }

    public void Reset(Material mat)
    {
        time = 0;
        this.mat = mat;
    }
}
