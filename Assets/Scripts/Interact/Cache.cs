using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    public Type type;
    public float maxDist = 450f;
    //public float ascendRate = 2f;
    public float sizeMin = 0.1f;
    public float sizemax = 1.0f;
    public float lifeTime = 45f;
    public AnimationCurve height;
    public float heightScale = 150f;
    float size;

    [Space]
    public GameObject prefab;
    public GameObject deathParticles;

    public static Dictionary<Type, int> nums = new Dictionary<Type, int>();

    //float lifeTime;
    float life;
    float startingHeight;

    private void Start()
    {
        size = Random.Range(sizeMin, sizemax);
        //transform.localScale = Vector3.one * size;
        transform.localScale = Vector3.zero;
        startingHeight = transform.position.y;
        float y = height.Evaluate(0);
        transform.position = transform.position.WithY(y * heightScale + startingHeight);
    }

    public void OnReelIn()
    {
        Destroy(gameObject);
        Audio audio = new Audio().SetPosition(transform.position).SetParent(Airship.instance.transform).SetDistance(50f);
        AudioManager.Play(audio.SetClip("Hit"));
        AudioManager.Play(audio.SetClip("Pop"));
        Airship.Spawn(Instantiate(prefab));

        Instantiate(deathParticles, transform.position, Quaternion.identity);
        //Airship.instance.kiddos.Add(obj.transform);
        //obj.transform.localScale = Vector3.one / Airship.instance.transform.localScale.x;
        //Airship.Fuel += fuel;
        //PopUp.Show($"+{Mathf.Round(fuel)} seconds of fuel: Total: {Mathf.Round(Airship.Fuel)}");
    }

    private void Update()
    {
        if (transform.position.DistanceFlat(Airship.instance.transform.position) > maxDist)
        {
            Destroy(gameObject);
            return;
        }

        float fac = life / lifeTime;

        if (life < 3f)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(0, size, Remap.Float(life, 0, 3, 0, 1));
        }
        else
        {
            transform.localScale = Vector3.one * size;
        }

        float y = height.Evaluate(fac);
        transform.position = transform.position.WithY(y * heightScale + startingHeight);
        //transform.position += Vector3.up * ascendRate * Time.deltaTime;

        life += Time.deltaTime;
        if (life > lifeTime)
        {
            //ascendRate -= Time.deltaTime * 3f;
            Destroy(gameObject);
        }

        //if (transform.position.y < -100)
        //    Destroy(gameObject);
    }

    private void OnEnable()
    {
        nums[type]++;
    }

    private void OnDisable()
    {
        nums[type]--;
    }

    public enum Type
    {
        Cargo,
        Fuel,
        Nox
    }

    public static int GetNum(Type t) => nums[t];
}
