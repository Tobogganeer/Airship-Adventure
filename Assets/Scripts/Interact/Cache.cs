using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    public float maxDist = 450f;
    public float ascendRate = 2f;
    public float sizeMin = 0.1f;
    public float sizemax = 1.0f;
    float size;

    [Space]
    public GameObject prefab;

    public static int Num { get; private set; }

    float lifeTime;

    private void Start()
    {
        size = Random.Range(sizeMin, sizemax);
        transform.localScale = Vector3.one * size;
    }

    public void OnReelIn()
    {
        Destroy(gameObject);
        Audio audio = new Audio().SetPosition(transform.position).SetParent(Airship.instance.transform).SetDistance(50f);
        AudioManager.Play(audio.SetClip("Hit"));
        AudioManager.Play(audio.SetClip("Pop"));
        GameObject obj = Instantiate(prefab, Airship.instance.spawnCrapHere.position,
            Airship.instance.spawnCrapHere.rotation);//, Airship.instance.transform);
        Airship.instance.kiddos.Add(obj.transform);
        //obj.transform.localScale = Vector3.one / Airship.instance.transform.localScale.x;
        //Airship.Fuel += fuel;
        //PopUp.Show($"+{Mathf.Round(fuel)} seconds of fuel: Total: {Mathf.Round(Airship.Fuel)}");
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
