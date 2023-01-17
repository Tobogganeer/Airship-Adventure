using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class DayNightController : MonoBehaviour
{
    public EnvBaker baker;
    public Material skyboxMaterial;

    [Range(0f, 1f)]
    public float timeOfDay;
    [ReadOnly] public float speed;
    public float secondsPerDay = 240;
    public float yRot = -30f;

    [Header("Rise -> Noon -> Set -> Midnight -> Rise")]
    public AnimationCurve dayIntensity;
    public AnimationCurve dayLightIntensity;
    public AnimationCurve nightIntensity;
    public AnimationCurve nightLightIntensity;
    public Gradient fogColour;
    public Gradient fogSunColour;
    public AnimationCurve fogExtraHeight;

    [Space]
    public LensFlareComponentSRP flare;
    public OD.AtmosphericFogRenderFeature atmosFog;
    public Light lightData;
    public float lightIntensityMult = 1.5f;
    public float flareIntensityMult = 0.5f;

    [Space]
    public float cubemapBakeTime = 5f;
    TimeSince bakeTime;

    private void Start()
    {
        bakeTime = 0;
    }

    const float DefaultFogHeight = 15f;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(timeOfDay * 360, yRot, 0);
        //float pos = Vector3.Dot(transform.forward, Vector3.up) * 0.5f + 0.5f;
        //Debug.Log(pos); // 0 : up, 0.5 : sideways, 1 : down

        if (skyboxMaterial != null)
        {
            //skyboxMaterial.SetFloat("_Mix", pos);
            skyboxMaterial.SetFloat("_DayIntensity", dayIntensity.Evaluate(timeOfDay));
            skyboxMaterial.SetFloat("_NightIntensity", nightIntensity.Evaluate(timeOfDay));
        }

        if (flare != null)
        {
            flare.intensity = dayLightIntensity.Evaluate(timeOfDay) * flareIntensityMult;
        }

        if (lightData != null)
        {
            lightData.intensity = dayLightIntensity.Evaluate(timeOfDay) * lightIntensityMult;
        }

        if (atmosFog?.settings != null)
        {
            atmosFog.settings.color = fogColour.Evaluate(timeOfDay);
            atmosFog.settings.sunColor = fogSunColour.Evaluate(timeOfDay);
            atmosFog.settings.fogHeightEnd = DefaultFogHeight + fogExtraHeight.Evaluate(timeOfDay);
        }

        if (Application.isPlaying)
        {
            timeOfDay = (timeOfDay + speed * Time.deltaTime) % 1;

            if (bakeTime > cubemapBakeTime)
            {
                //Debug.Log("Bake");
                bakeTime = 0;
                baker.Bake();
            }
        }
    }

    private void OnValidate()
    {
        speed = 1f / secondsPerDay;
        //secondsPerDay = 1f / speed;
    }
}
