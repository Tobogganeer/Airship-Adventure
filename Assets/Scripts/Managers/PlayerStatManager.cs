using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

[System.Serializable]
public class PlayerStats
{
    public int cash;
}

public class PlayerStatManager : MonoBehaviour
{
    private static PlayerStats stats;

    public int cash;


    public static PlayerStatManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;

        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(AutoSave());
        CurrentStatsLoad();
    }

    private void Update()
    {
        if (Keyboard.current.backquoteKey.wasPressedThisFrame)
        {
            PopUp.Show("DEBUG: Started Saving...", 1);
            CurrentStatsSave();
        }
    }

    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(300);
        CurrentStatsSave();

    }
    public static PlayerStats CurrentStats
    {
        get
        {
            if (stats == null)
                stats = new PlayerStats();

            return stats;
        }
    }

    public static void Save()
    {
        try
        {
            WriteSave();
            PopUp.Show("DEBUG: Saved!", 3);
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error saving save: " + ex);
        }
    }

    public static void Load()
    {
        try
        {
            ReadSave();
        }
        catch (System.Exception ex)
        {
            Debug.Log("Error reading save: " + ex);
        }
    }

    public void CurrentStatsLoad()
    {
        Load();
        PlayerStats s = CurrentStats;
        cash = s.cash;
    }

    public void CurrentStatsSave()
    {
        PlayerStats s = CurrentStats;
        s.cash = cash;
        Save();
    }

    private static void WriteSave()
    {
        SaveLoad.SaveJson("save.json", stats ?? new PlayerStats(), true);
    }

    private static void ReadSave()
    {
        stats = SaveLoad.ReadJson<PlayerStats>("save.json", true);
        if (stats == null)
        {
            stats = new PlayerStats();
            WriteSave();
        }
    }
}
