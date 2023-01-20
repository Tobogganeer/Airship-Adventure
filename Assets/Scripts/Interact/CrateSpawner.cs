using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public GameObject cratePrefab;
    public float spawnRange = 100f;

    Transform trackedCrate;

    [Space]
    public Mesh crateMesh;

    private void Update()
    {
        if (trackedCrate == null && transform.position.Distance(PlayerMovement.Position) > spawnRange)
        {
            trackedCrate = Instantiate(cratePrefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0)).transform;
        }
        else
        {
            if (trackedCrate.position.Distance(transform.position) > spawnRange)
            {
                trackedCrate = null;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);

        if (crateMesh)
        {
            Gizmos.DrawMesh(crateMesh, transform.position, Quaternion.identity);
        }
    }
}
