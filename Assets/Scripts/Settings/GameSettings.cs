using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public float sens;
    public float quality;
    public int   fov;
    public float master;
    public float ambient;
    public float sfx;

    public GameSettings()
    {
        sens = 20f;
        quality = 1.0f;
        fov = 75;
        master = 0.5f;
        ambient = 0.5f;
        sfx = 1.0f;
    }

    public void Validate()
    {
        sens = Mathf.Clamp(sens, 10, 50);
        quality = Mathf.Clamp(quality, 0.5f, 1.5f);
        fov = Mathf.Clamp(fov, 50, 110);
        master = Mathf.Clamp(master, 0, 1);
        ambient = Mathf.Clamp(ambient, 0, 1);
        sfx = Mathf.Clamp(sfx, 0, 1);
    }
}
