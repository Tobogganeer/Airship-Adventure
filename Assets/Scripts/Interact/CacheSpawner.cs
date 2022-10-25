using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheSpawner : MonoBehaviour
{
    public GameObject cachePrefab;
    public Camera cam;
    public float maxSpawnRange = 250f;
    public float minSpawnRange = 150f;
    public int caches = 4;
    public Vector2 spawnTime = new Vector2(5f, 20f);
    Plane[] planes = new Plane[6];

    float timer;


    private bool IsVisible(Camera c, Renderer target)
    {
        GeometryUtility.CalculateFrustumPlanes(c, planes);
        return GeometryUtility.TestPlanesAABB(planes, target.bounds);
    }

    private void Update()
    {
        //while (Cache.Num < caches)
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = Random.Range(spawnTime.x, spawnTime.y);

            if (Cache.Num < caches)
            {
                Vector3 pos = Random.insideUnitCircle.XYToXZ() * maxSpawnRange;
                if (pos.magnitude < minSpawnRange)
                    pos = pos.normalized * minSpawnRange;
                pos.y = Random.Range(-5f, 25f);

                GameObject newCache = Instantiate(cachePrefab, pos + transform.position, Quaternion.Euler(0, Random.value * 360f, 0));
                if (IsVisible(cam, newCache.GetComponent<Renderer>()))
                {
                    pos.x = -pos.x;
                    pos.z = -pos.z;
                    newCache.transform.position = pos + transform.position;
                }

                Vector3 audioPos = Airship.instance.transform.position +
                    Airship.instance.transform.position.DirectionTo(newCache.transform.position) * 20f;
                AudioManager.Play(new Audio("Hit").SetPosition(audioPos).SetVolume(0.2f).SetDistance(100f));
            }
        }
    }
}
