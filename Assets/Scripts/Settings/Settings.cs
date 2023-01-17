using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    private static GameSettings settings;
    public static GameSettings CurrentSettings
    {
        get
        {
            if (settings == null)
                settings = new GameSettings();

            return settings;
        }
    }

    public static void Save()
    {
        try
        {
            WriteSettings();
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error saving settings: " + ex);
        }
    }

    public static void Load()
    {
        try
        {
            ReadSettings();
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error reading settings: " + ex);
        }
    }

    private static void WriteSettings()
    {
        SaveLoad.SaveJson("settings.json", settings ?? new GameSettings(), true);
    }

    private static void ReadSettings()
    {
        settings = SaveLoad.ReadJson<GameSettings>("settings.json", true);
        if (settings == null)
        {
            settings = new GameSettings();
            WriteSettings();
        }

        settings.Validate();
    }
}
