using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    public CanvasGroup panel;

    [Space]
    public SettingEntry sens;
    public SettingEntry quality;
    public SettingEntry fov;
    public SettingEntry master;
    public SettingEntry ambient;
    public SettingEntry sfx;

    [Space]
    public UniversalRenderPipelineAsset urpAsset;

    bool active = false;

    void Start()
    {
        panel.alpha = 0;
        QualityManager.SetMaxFramerate(144);
        QualityManager.SetVSync(false);

        Load();
    }

    void Update()
    {
        panel.alpha = Mathf.Lerp(panel.alpha, active ? 1f : 0f, Time.deltaTime * 15f);
        panel.interactable = active;

        FPSCamera.SettingsSensitivity = sens.Value;
        urpAsset.renderScale = quality.Value;
        FPSCamera.SettingsFOV = fov.Value;

        AudioMaster.SetMasterVolume(master.Value);
        AudioMaster.SetAmbientVolume(ambient.Value);
        AudioMaster.SetSFXVolume(sfx.Value);
    }


    public void Press()
    {
        active = !active;
    }

    public void Save()
    {
        GameSettings s = Settings.CurrentSettings;
        s.sens = sens.Value;
        s.quality = quality.Value;
        s.fov = (int)fov.Value;
        s.master = master.Value;
        s.ambient = ambient.Value;
        s.sfx = sfx.Value;
        Settings.Save();
    }

    public void Load()
    {
        Settings.Load();
        GameSettings s = Settings.CurrentSettings;
        sens.Value = s.sens;
        quality.Value = s.quality;
        fov.Value = s.fov;
        master.Value = s.master;
        ambient.Value = s.ambient;
        sfx.Value = s.sfx;
    }
}
