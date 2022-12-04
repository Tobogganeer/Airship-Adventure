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
    AlarmProfile activeAlarm;

    public AudioSource alarmSource;

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
        terrainMaterial.SetColor(EmStr, nearTerrain && !disableAlarms ? terrainColour : black);
    }

    void SetAlarms()
    {
        AlarmProfile old = activeAlarm;

        if (disableAlarms)
            activeAlarm = null;

        else if (nearTerrain)
            activeAlarm = terrainAlarm;

        else if (fuel < fuelThreshold)
            activeAlarm = fuelAlarm;

        else if (canDock)
            activeAlarm = dockAlarm;

        else
            activeAlarm = null;

        if (activeAlarm != old)
            activeAlarm?.Reset(alarmSource);

        activeAlarm?.Tick(Time.deltaTime);
    }

    void SetLights()
    {

    }
}
