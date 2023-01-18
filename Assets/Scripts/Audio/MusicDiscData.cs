using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDiscData : MonoBehaviour
{
    private static MusicDiscData instance;
    private void Awake()
    {
        instance = this;
    }

    public MusicData[] allDiscs;

    public List<int> discsOnShip;

    private void Start()
    {
        for (int i = 0; i < allDiscs.Length; i++)
        {
            allDiscs[i].discIndex = i;
        }
    }


    public static MusicData Get(int disc)
    {
        return instance.allDiscs[disc];
    }

    public static int GetNewDisc()
    {
        List<int> possibilities = new List<int>();

        for (int i = 0; i < instance.allDiscs.Length; i++)
        {
            possibilities.Add(i);
        }

        possibilities.RemoveAll((disc) => instance.discsOnShip.Contains(disc));

        return possibilities[Random.Range(0, possibilities.Count)];
    }

    public static void Spawned(int disc)
    {
        instance.discsOnShip.Add(disc);
    }

    public static void Despawned(int disc)
    {
        instance.discsOnShip.Remove(disc);
    }
}

[System.Serializable]
public class MusicData
{
    [HideInInspector]
    public int discIndex;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    public Color discColour;
}

