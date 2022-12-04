using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipCrash : MonoBehaviour
{
    [Layer]
    public int terrainLayer;
    int layerMask;
    public float alarmRange = 150f;
    public float alarmRadius = 5f;
    public static bool NearTerrain;
    public static bool NearTerrainVertical;

    [Space]
    public Transform vertDetector;
    public float vertRange = 20;

    [Space]
    public bool drawGizmos = true;

    private void Start()
    {
        layerMask = 1 << terrainLayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == terrainLayer)
            Airship.Crash("Crashed into terrain!", 3f);
    }

    private void Update()
    {
        NearTerrain = Physics.SphereCast(new Ray(transform.position, transform.forward), alarmRadius, alarmRange, layerMask);
        NearTerrainVertical = Physics.SphereCast(new Ray(vertDetector.position, vertDetector.forward), alarmRadius, vertRange, layerMask);
    }


    //if (Physics.CheckSphere(transform.position, alarmRangeVertical, terrainLayer))
    //    NearTerrain = true;
    //else if (Physics.OverlapSphereNonAlloc(transform.position, alarmRangeHorizontal, terrain, terrainLayer) > 0)
    //{
    //    Vector3 pt = terrain[0].ClosestPoint(transform.position);
    //}

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(transform.position, alarmRangeHorizontal);
        //Gizmos.DrawLine(transform.position + Vector3.up * alarmRangeVertical,
        //    transform.position + Vector3.down * alarmRangeVertical);
        Gizmos.DrawWireSphere(transform.position, alarmRadius);
        Gizmos.DrawWireSphere(transform.position + transform.forward * alarmRange, alarmRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * alarmRange);

        if (vertDetector != null)
        {
            Gizmos.DrawWireSphere(vertDetector.position, alarmRadius);
            Gizmos.DrawWireSphere(vertDetector.position + vertDetector.forward * vertRange, alarmRadius);
            Gizmos.DrawLine(vertDetector.position, vertDetector.position + vertDetector.forward * vertRange);
        }
    }
}
