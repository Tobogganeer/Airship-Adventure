using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheSpawner : MonoBehaviour
{
    //public GameObject[] cachePrefab;

    //public Camera cam;
    //Camera cam;
    public float maxSpawnRange = 250f;
    public float minSpawnRange = 150f;
    //public int caches = 3;
    public SerializableDictionary<Cache.Type, int> maxes;
    public SerializableDictionary<Cache.Type, GameObject> cachePrefabs;
    public Vector2 spawnTime = new Vector2(5f, 20f);
    //Plane[] planes = new Plane[6];

    float timer;

    // Relics of airshit game VVV

    //private void Start()
    //{
    //    cam = FPSCamera.instance.GetComponent<Camera>();
    //}

    //private bool IsVisible(Camera c, Renderer target)
    //{
    //    GeometryUtility.CalculateFrustumPlanes(c, planes);
    //    return GeometryUtility.TestPlanesAABB(planes, target.bounds);
    //}

    private void Start()
    {
        Cache.nums = new Dictionary<Cache.Type, int>();
        Cache.nums.Add(Cache.Type.Fuel, 0);
        Cache.nums.Add(Cache.Type.Cargo, 0);
        Cache.nums.Add(Cache.Type.Nox, 0);
    }

    private void Update()
    {
        //while (Cache.Num < caches)
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = Random.Range(spawnTime.x, spawnTime.y);
            // Could be a jerk and prevent fuel caches spawning when docked

            Cache.Type type = (Cache.Type)Random.Range(0, 3);
            if (Airship.Fuel01 < 0.1f)
                type = Cache.Type.Fuel;
            int rolls = 2;

            while (rolls > 0)
            {
                if (Cache.GetNum(type) < maxes[type])
                {
                    Vector3 pos = Random.insideUnitCircle.XYToXZ() * maxSpawnRange;
                    if (pos.magnitude < minSpawnRange)
                        pos = pos.normalized * minSpawnRange;
                    pos.y = Random.Range(-5f, 25f);

                    GameObject newCache = Instantiate(cachePrefabs[type], pos + transform.position, Quaternion.Euler(0, Random.value * 360f, 0));
                    //if (IsVisible(cam, newCache.GetComponent<Renderer>()))
                    //{
                    //pos.x = -pos.x;
                    //pos.z = -pos.z;
                    //newCache.transform.position = pos + transform.position;
                    //}

                    Vector3 audioPos = Airship.instance.transform.position +
                        Airship.instance.transform.position.DirectionTo(newCache.transform.position) * 20f;
                    AudioManager.Play(new Audio("Hit").SetPosition(audioPos).SetVolume(0.2f).SetDistance(100f));

                    return;
                }

                rolls--;
                Cache.Type t = type;
                type = (Cache.Type)(((int)type + 1) % 3);
                //Debug.Log("Skipped to " + type + " from " + t);
            }
        }
    }
}

[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{
    public List<SerializedPair<TKey, TValue>> values;

    Dictionary<TKey, TValue> _dict;
    public Dictionary<TKey, TValue> dictionary 
    { 
        get
        {
            if (_dict == null)
            {
                _dict = new Dictionary<TKey, TValue>();
                foreach (SerializedPair<TKey, TValue> pair in values)
                {
                    _dict.Add(pair.key, pair.value);
                }
            }

            return _dict;
        }
    }

    public TValue this[TKey key]
    {
        get => dictionary[key];
        set => dictionary[key] = value;
    }

    [System.Serializable]
    public class SerializedPair<T1, T2>
    {
        public T1 key;
        public T2 value;
    }
}
