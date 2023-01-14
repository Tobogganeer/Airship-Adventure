using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public AudioClip clip;
    public float volume = 1.0f;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Pickup pickup;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pickup = GetComponent<Pickup>();
    }
}
