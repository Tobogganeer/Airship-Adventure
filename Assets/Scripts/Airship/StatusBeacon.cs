using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBeacon : MonoBehaviour
{
    bool canDock => Airship.CanDock;
    bool docked => Airship.Docked;
    bool docking => Airship.Docking;
    bool releasing => DockingSystem.RecentlyDocked;
    float fuel => Airship.Fuel01;
    bool nearTerrain => AirshipCrash.NearTerrain;
    bool nearTerrainVertical => AirshipCrash.NearTerrainVertical;

    public float fuelThreshold = 0.1f;

    [Header("Material indices")]
    public int dockIndicator;
    public int fuelIndicator;
    public int terrainIndicator;
    public int statusLight;

    Material[] mats;
    Material dockMaterial;
    Material fuelMaterial;
    Material terrainMaterial;
    Material lightMaterial;

    [Space]
    [ColorUsage(false, true)] public Color dockColour;
    [ColorUsage(false, true)] public Color fuelColour;
    [ColorUsage(false, true)] public Color terrainColour;
    [ColorUsage(false, true)] public Color black;

    [Space]
    public AlarmProfile dockAlarm;
    public AlarmProfile fuelAlarm;
    public AlarmProfile terrainAlarm;
    public AlarmProfile terrainAlarmVertical;
    AlarmProfile activeAlarm;

    public AudioSource alarmSource;

    [Space]
    public AlarmLightProfile terrainLP;
    public AlarmLightProfile terrainVerticalLP; // Slightly less annoying alarm for up and down
    public AlarmLightProfile fuelLP;
    public AlarmLightProfile canDockLP;
    public AlarmLightProfile dockingLP;
    public AlarmLightProfile dockedLP;
    public AlarmLightProfile blankLP;
    AlarmLightProfile activeLP;

    const string EmStr = "_EmissionColor"; // Color, not Colour :|

    bool disableAlarms => docked || docking || releasing;

    private void Start()
    {
        mats = GetComponent<Renderer>().materials;
        dockMaterial = mats[dockIndicator];
        fuelMaterial = mats[fuelIndicator];
        terrainMaterial = mats[terrainIndicator];
        lightMaterial = mats[statusLight];
    }
    private void OnDestroy()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            Destroy(mats[i]);
        }
        //Destroy(dockMaterial);
        //Destroy(fuelMaterial);
        //Destroy(terrainMaterial);
        //Destroy(lightMaterial);
    }

    private void Update()
    {
        SetIndicators();
        SetAlarms();
        SetLights();
    }

    void SetIndicators()
    {
        dockMaterial.SetColor(EmStr, canDock || docking || docked ? dockColour : black);
        fuelMaterial.SetColor(EmStr, fuel < fuelThreshold ? fuelColour : black);
        terrainMaterial.SetColor(EmStr, (nearTerrain || nearTerrainVertical) && !disableAlarms ? terrainColour : black);
    }

    void SetAlarms()
    {
        AlarmProfile old = activeAlarm;

        if (disableAlarms) // Docked/docking
            activeAlarm = null;
        else if (nearTerrain) // Gonna crash
            activeAlarm = terrainAlarm;
        else if (nearTerrainVertical) // Gonna crash but down
            activeAlarm = terrainAlarmVertical;
        else if (fuel < fuelThreshold) // Low fuel
            activeAlarm = fuelAlarm;
        else if (canDock) // Near docking station
            activeAlarm = dockAlarm;
        else
            activeAlarm = null;

        if (activeAlarm != old)
            activeAlarm?.Reset(alarmSource);

        activeAlarm?.Tick(Time.deltaTime);
    }

    void SetLights()
    {
        AlarmLightProfile old = activeLP;

        //terrainLP;
        //fuelLP;
        //canDockLP;
        //dockingLP;
        //dockedLP;

        if (nearTerrain && !disableAlarms) // Near terrain and not docked/docking
            activeLP = terrainLP;
        else if (nearTerrainVertical && !disableAlarms) // Near terrain vertically and not docked/docking
            activeLP = terrainVerticalLP;
        else if (fuel < fuelThreshold) // Low fuel
            activeLP = fuelLP;
        else if (canDock)
            activeLP = canDockLP;
        else if (docking)
            activeLP = dockingLP;
        else if (docked)
            activeLP = dockedLP;
        else
            activeLP = blankLP;

        if (activeLP != old)
            activeLP?.Reset(lightMaterial);

        activeLP?.Tick(Time.deltaTime);
    }

    [System.Serializable]
    public class AlarmProfile
    {
        public AudioClip clip;
        public float audioPeriod = 1f;
        public float volume = 1f;

        float time;
        AudioSource audioSource;

        public void Tick(float dt)
        {
            time += dt;
            if (time > audioPeriod)
            {
                time = 0;
                audioSource.Play();
            }
        }

        public void Reset(AudioSource alarmSource)
        {
            time = audioPeriod; // Set to 0 and the lights will flash once before the sound plays
            audioSource = alarmSource;
            audioSource.clip = clip;
            audioSource.volume = volume;
        }
    }
}
