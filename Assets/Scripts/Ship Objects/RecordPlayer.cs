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

    [Space]
    public float backgroundDistanceMin = 4f;
    public float backgroundDistanceMax = 10f;

    float distortion;
    float lowPass;
    float highPass;

    const float LowMax = 22000;

    AudioLowPassFilter low; // >
    AudioHighPassFilter high; // <
    AudioDistortionFilter dist; // <

    private void Start()
    {
        low = GetComponent<AudioLowPassFilter>();
        high = GetComponent<AudioHighPassFilter>();
        dist = GetComponent<AudioDistortionFilter>();

        distortion = dist.distortionLevel;
        lowPass = low.cutoffFrequency;
        highPass = high.cutoffFrequency;
    }


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

        float dist = transform.position.Distance(PlayerMovement.Position);
        dist = Mathf.Clamp(dist, backgroundDistanceMin, backgroundDistanceMax);
        dist = Remap.Float(dist, backgroundDistanceMin, backgroundDistanceMax, 0, 1);

        low.cutoffFrequency = Mathf.Lerp(lowPass, LowMax, dist);
        high.cutoffFrequency = Mathf.Lerp(highPass, 0, dist);
        this.dist.distortionLevel = Mathf.Lerp(distortion, 0, dist);
        audioSource.spatialBlend = 1 - dist;
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
            MusicData d = MusicDiscData.Get(currentDisk.discIndex);
            audioSource.clip = d.clip;
            audioSource.volume = d.volume;
            audioSource.Play();
            Music.Stop();
            CancelInvoke();
            Invoke(nameof(PlayMusic), audioSource.clip.length + 1f);
            Invoke(nameof(Eject), audioSource.clip.length + 1f);
        }
    }

    void PlayMusic()
    {
        Music.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, backgroundDistanceMin);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, backgroundDistanceMax);
    }
}
