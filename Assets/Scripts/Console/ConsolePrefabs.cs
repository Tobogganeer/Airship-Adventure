using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePrefabs : MonoBehaviour
{
    public static ConsolePrefabs instance;
    private void Awake()
    {
        instance = this;
    }

    public SerializableDictionary<string, GameObject> prefabs = new SerializableDictionary<string, GameObject>();
}
