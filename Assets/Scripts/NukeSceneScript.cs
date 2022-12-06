using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NukeSceneScript : MonoBehaviour
{
    public GameObject nukeVFX;
    public AudioClip nukeSFX;
    public GameObject spawnNuke;

    //private AudioSource audioSource;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            Nuke();
        }        
    }

    void Nuke()
    {
        AudioManager.Play(new Audio("Nuke").SetPosition(transform.position).SetParent(transform));
        GameObject vfxNuke = Instantiate(nukeVFX, spawnNuke.transform.position, spawnNuke.transform.rotation) as GameObject;
        vfxNuke.transform.localScale = new Vector3(6, 6, 6);
        Destroy(vfxNuke, 15);
    }
}
