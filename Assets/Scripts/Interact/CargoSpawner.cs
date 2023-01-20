using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoSpawner : MonoBehaviour
{
    public GameObject[] trinkets;
    public GameObject disc;
    public GameObject crate;

    [Range(0f, 1f)] public float trinketSpawnChance = 0.8f;
    [Range(0f, 1f)] public float discSpawnChance = 0.75f;
    [Range(0f, 1f)] public float crateSpawnChance = 0.33f;

    void Start()
    {
        Spawn();
        Destroy(gameObject);
    }

    void Spawn()
    {
        if (Random.value < trinketSpawnChance)
            Airship.Spawn(Instantiate(trinkets[Random.Range(0, trinkets.Length)]));
        if (Random.value < discSpawnChance)
            Airship.Spawn(Instantiate(disc));
        if (Random.value < crateSpawnChance)
            Airship.Spawn(Instantiate(crate));
        // Airship.Spawn(obj);
    }
}
