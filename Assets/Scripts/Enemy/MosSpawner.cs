using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    //public Camera cam;
    Camera cam;
    public float maxSpawnRange = 250f;
    public float minSpawnRange = 150f;
    public int enemys = 2;
    public Vector2 spawnTime = new Vector2(2f, 10f);
    Plane[] planes = new Plane[6];

    float timer;

    private void Start()
    {
        cam = FPSCamera.instance.GetComponent<Camera>();
    }

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

            if (Mos.NumEnemys < enemys)
            {
                Vector3 pos = Random.insideUnitCircle.XYToXZ() * maxSpawnRange;
                if (pos.magnitude < minSpawnRange)
                    pos = pos.normalized * minSpawnRange;
                pos.y = Random.Range(-5f, 25f);

                GameObject newEnemy = Instantiate(enemyPrefab, pos + transform.position, Quaternion.Euler(0, 0, 0));
                if (IsVisible(cam, newEnemy.GetComponent<Renderer>()))
                {
                    pos.y = -pos.y;
                    pos.x = -pos.x;
                    pos.z = -pos.z;
                    newEnemy.transform.position = pos + transform.position;
                }

                /* Vector3 audioPos = Airship.instance.transform.position +
                    Airship.instance.transform.position.DirectionTo(newEnemy.transform.position) * 20f;
                AudioManager.Play(new Audio("Hit").SetPosition(audioPos).SetVolume(0.2f).SetDistance(100f)); */
            }
        }
    }
}
