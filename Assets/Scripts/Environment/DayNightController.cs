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
    //public float yRot = -30f;

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

    [Space]
    public GameObject desertSmoke;
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
        //transform.localRotation = Quaternion.Euler(timeOfDay * 360, yRot, 0);
        transform.localRotation = Quaternion.Euler(timeOfDay * 360, 0, 0);
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
            heightFog.settings.sunColor = Add(fogGrad.Evaluate(timeOfDay), fogSunColour.Evaluate(timeOfDay));
            heightFog.settings.fogHeightEnd = DefaultFogHeight + fogExtraHeight.Evaluate(timeOfDay)
                + (currentBiome == Biome.Desert ? desertFogExtraHeight : 0);
            if (currentBiome == Biome.Desert)
            {
                heightFog.settings.fogHeightPower = 1f;
                borderFog.settings.fogDensityPower = 0.2f;
                if (Application.isPlaying)
                    desertSmoke.SetActive(true);
            }
            else
            {
                heightFog.settings.fogHeightPower = 0.2f;
                borderFog.settings.fogDensityPower = 0.75f;
                if (Application.isPlaying)
                    desertSmoke.SetActive(false);
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
            //borderFog.settings.sunColor = fogSunColour.Evaluate(timeOfDay);
            borderFog.settings.sunColor = Add(fogGrad.Evaluate(timeOfDay), fogSunColour.Evaluate(timeOfDay));
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

    Color Add(Color col1, Color col2)
    {
        float _1 = col1.a;
        float _2 = col2.a;

        return new Color(
            Mathf.Clamp01(col1.r * _1 + col2.r * _2),
            Mathf.Clamp01(col1.g * _1 + col2.g * _2),
            Mathf.Clamp01(col1.b * _1 + col2.b * _2),
            Mathf.Clamp01(_1 + _2)
        );
    }
}
