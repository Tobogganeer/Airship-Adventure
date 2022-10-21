using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    public float maxDist = 450f;
    public float ascendRate = 2f;
    public float sizeMin = 0.1f;
    public float sizemax = 1.0f;

    [Header("Touch these for balance")]
    public float fuelMin = 15f;
    public float fuelMax = 45f;

    public static int Num { get; private set; }
    float fuel;

    float lifeTime;

    private void Start()
    {
        fuel = Random.Range(fuelMin, fuelMax);
        transform.localScale = Vector3.one * Remap.Float(fuel, fuelMin, fuelMax, sizeMin, sizemax);
        //Destroy(gameObject, 35f);
    }

    public void OnReelIn()
    {
        Destroy(gameObject);
        Audio audio = new Audio().SetPosition(transform.position).SetParent(Airship.instance.transform).SetDistance(50f);
        AudioManager.Play(audio.SetClip("Hit"));
        AudioManager.Play(audio.SetClip("Pop"));
        Airship.Fuel += fuel;
        PopUp.Show($"+{Mathf.Round(fuel)} seconds of fuel: Total: {Mathf.Round(Airship.Fuel)}");
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Airship.instance.transform.position) > maxDist)
            Destroy(gameObject);
        transform.position += Vector3.up * ascendRate * Time.deltaTime;

        lifeTime += Time.deltaTime;
        if (lifeTime > 45f)
        {
            ascendRate -= Time.deltaTime * 3f;
        }

        if (transform.position.y < -100)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Num++;
    }

    private void OnDisable()
    {
        Num--;
    }
}
