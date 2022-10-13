using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public float sensitivity;

    public int masterVolume;
    public int sfxVolume;
    public int ambientVolume;

    public int maxFramerate;
    public bool vsync;

    public GameSettings()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        sensitivity = 50;
        masterVolume = 65;
        sfxVolume = 65;
        ambientVolume = 20;
        maxFramerate = 144;
        vsync = false;
    }

    public void Validate()
    {
        sensitivity = Mathf.Clamp(sensitivity, 0, 100);

        masterVolume = Mathf.Clamp(masterVolume, 0, 100);

        sfxVolume = Mathf.Clamp(sfxVolume, 0, 100);

        ambientVolume = Mathf.Clamp(ambientVolume, 0, 100);

        maxFramerate = Mathf.Clamp(maxFramerate, 30, 300);
    }
}
