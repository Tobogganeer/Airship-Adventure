using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public int discIndex;
    //public AudioClip clip;
    //public float volume = 1.0f;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Pickup pickup;
    Material mat;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pickup = GetComponent<Pickup>();

        discIndex = MusicDiscData.GetNewDisc();

        MusicDiscData.Spawned(discIndex);

        mat = GetComponent<Renderer>().materials[1];
        mat.color = MusicDiscData.Get(discIndex).discColour;
    }

    //private void OnEnable()
    //{
    //    MusicDiscData.Spawned(discIndex);
    //}

    private void OnDisable()
    {
        MusicDiscData.Despawned(discIndex);
    }

    private void OnDestroy()
    {
        Destroy(mat);
    }
}
