using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class DayNightController : MonoBehaviour
{
    public static DayNightController instance;
    private void Awake()
    {
        instance = this;
    }

    public EnvBaker baker;
    public Material skyboxMaterial;

    [Range(0f, 1f)]
    public float timeOfDay;
    [ReadOnly] public float speed;
    public float secondsPerDay = 240;
    public float yRot = -30f;

    [Header("Rise -> Noon -> Set -> Midnight -> Rise")]
    public AnimationCurve sunriseSkyboxIntensity;
    public AnimationCurve sunsetSkyboxIntensity;
    public AnimationCurve middaySkyboxIntensity;
    public AnimationCurve nightSkyboxIntensity;
    public AnimationCurve dayLightIntensity;
    //public Gradient fogColour;
    public Gradient fogColourGrasslands;
    public Gradient fogColourDesert;
    public Gradient fogColourSnow;
    public Gradient borderFogColourGrasslands;
    public Gradient borderFogColourDesert;
    public Gradient borderFogColourSnow;
    public Gradient fogSunColour;
    public AnimationCurve fogExtraHeight;
    public float desertFogExtraHeight = 30;

    [Space]
    public LensFlareComponentSRP flare;
    public OD.AtmosphericFogRenderFeature heightFog;
    public OD.AtmosphericFogRenderFeature borderFog;
    public Light lightData;
    public float lightIntensityMult = 1.5f;
    public float flareIntensityMult = 0.5f;

    [Space]
    public float cubemapBakeAngle = 0.5f;
    //public float cubemapBakeTime = 5f;
    //TimeSince bakeTime;
    //Vector3 bakeAngle;
    float bakeAngle;
    float bakeTimer;

    static Biome currentBiome
    {
        get
        {
            if (ProcGen.instance == null) return Biome.Grasslands;
            return ProcGen.instance.currentBiome;
        }
    }

    private void Start()
    {
        //bakeTime = 0;
        bakeAngle = timeOfDay * 360;
        heightFog.SetActive(true);
        borderFog.SetActive(true);
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
            skyboxMaterial.SetFloat("_SunriseIntensity", sunriseSkyboxIntensity.Evaluate(timeOfDay));
            skyboxMaterial.SetFloat("_SunsetIntensity", sunsetSkyboxIntensity.Evaluate(timeOfDay));
            skyboxMaterial.SetFloat("_NightIntensity", nightSkyboxIntensity.Evaluate(timeOfDay));
            skyboxMaterial.SetFloat("_MiddayIntensity", middaySkyboxIntensity.Evaluate(timeOfDay));
        }

        if (flare != null)
        {
            flare.intensity = dayLightIntensity.Evaluate(timeOfDay) * flareIntensityMult;
        }

        if (lightData != null)
        {
            lightData.intensity = dayLightIntensity.Evaluate(timeOfDay) * lightIntensityMult;
        }

        if (heightFog?.settings != null)
        {
            Gradient fogGrad = currentBiome switch
            {
                Biome.Grasslands => fogColourGrasslands,
                Biome.Desert => fogColourDesert,
                Biome.Snow => fogColourSnow,
                _ => throw new System.NotImplementedException(),
            };

            heightFog.settings.color = fogGrad.Evaluate(timeOfDay);
            heightFog.settings.sunColor = fogSunColour.Evaluate(timeOfDay);
            heightFog.settings.fogHeightEnd = DefaultFogHeight + fogExtraHeight.Evaluate(timeOfDay)
                + (currentBiome == Biome.Desert ? desertFogExtraHeight : 0);
            if (currentBiome == Biome.Desert)
            {
                heightFog.settings.fogHeightPower = 1f;
            }
            else
            {
                heightFog.settings.fogHeightPower = 0.2f;
            }
        }

        if (borderFog?.settings != null)
        {
            Gradient fogGrad = currentBiome switch
            {
                Biome.Grasslands => borderFogColourGrasslands,
                Biome.Desert => borderFogColourDesert,
                Biome.Snow => borderFogColourSnow,
                _ => throw new System.NotImplementedException(),
            };
            borderFog.settings.color = fogGrad.Evaluate(timeOfDay);
            borderFog.settings.sunColor = fogSunColour.Evaluate(timeOfDay);
        }

        if (Application.isPlaying)
        {
            timeOfDay = (timeOfDay + speed * Time.deltaTime) % 1;
            bakeTimer -= Time.deltaTime;

            //if (bakeTime > cubemapBakeTime)
            if (timeOfDay * 360 - bakeAngle > cubemapBakeAngle || bakeTimer < 0)
            {
                //Debug.Log("Bake");
                //bakeTime = 0;
                bakeAngle = timeOfDay * 360;
                bakeTimer = 5f;
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
