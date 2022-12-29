using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayer : MonoBehaviour, IInteractable
{
    public Transform recordHolder;
    public Record currentDisk;
    public AudioSource audioSource;
    public float spinSpeed = 120;
    public float ejectForce = 10;


    public bool FixedPosition => false;
    public bool IsInteracting { get; set; }
    public Transform InteractFrom => throw new System.NotImplementedException();

    public void OnInteract()
    {
        Eject();
    }

    private void Update()
    {
        if (currentDisk != null)
        {
            currentDisk.transform.position = recordHolder.position;
            currentDisk.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
        }
    }

    void Eject()
    {
        if (currentDisk != null)
        {
            currentDisk.rb.AddForce(recordHolder.forward * ejectForce, ForceMode.Impulse);
            currentDisk = null;
            audioSource.Stop();
            Music.Play();
            CancelInvoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.HasTag("Record") && currentDisk == null)
        {
            currentDisk = other.GetComponent<Record>();
            currentDisk.pickup.IsInteracting = false;
            currentDisk.transform.position = recordHolder.position;
            currentDisk.rb.velocity = Vector3.zero;
            currentDisk.rb.angularVelocity = Vector3.zero;
            currentDisk.transform.rotation = Quaternion.identity;
            audioSource.clip = currentDisk.clip;
            audioSource.volume = currentDisk.volume;
            audioSource.Play();
            Music.Stop();
            CancelInvoke();
            Invoke(nameof(PlayMusic), audioSource.clip.length + 1f);
        }
    }

    void PlayMusic()
    {
        Music.Play();
    }
}
