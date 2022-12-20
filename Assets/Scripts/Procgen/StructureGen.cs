using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGen : MonoBehaviour
{
    [Range(0, 10)] public int coastalStructures = 3;
    [Range(0, 15)] public int inlandStructures = 5;

    [Space]
    public float seaLevel = 50;
    public float radius = 2500;
    public float minStructureDist = 250;
    public LayerMask layerMask;

    [Space]
    public float minCoastalStructureAngle = 20;

    [Space]
    public float maxHeight = 100;
    public float sphereCastRadius = 10f;

    [Space]
    public GameObject[] coastalStructurePrefabs;
    public GameObject[] inlandStructurePrefabs;

    [Space]
    public bool gizmos;

    Transform holder;

    //[SerializeField, HideInInspector]
    //List<GameObject> structures = new List<GameObject>();

    public void Generate(int seed)
    {
        //Random.State oldState = Random.state;

        //Debug.Log(transform.childCount + " children");
        //int attempts = 100;
        //while (transform.childCount > 0 && attempts > 0)
        //{
        //    attempts--;
        //    if (Application.isPlaying)
        //        Destroy(transform.GetChild(0).gameObject);
        //    else
        //        DestroyImmediate(transform.GetChild(0).gameObject);
        //}

        //if (!Application.isPlaying)
        //{
        //    StartCoroutine(GenerateOverTime(seed));
        //}
        //else
        //{
        //if (holder != null)
        if (transform.childCount > 0)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(0).gameObject);
            else
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        holder = new GameObject("POI Holder").transform;
        holder.SetParent(transform);
        holder.localPosition = Vector3.zero;
        holder.localRotation = Quaternion.identity;
        holder.localScale = Vector3.one;

        //Debug.Log("Attempts left: " + attempts);

        //foreach (GameObject s in structures)
        //    if (s != null)
        //        if (Application.isPlaying)
        //            Destroy(s);
        //        else
        //            DestroyImmediate(s);

        //foreach (GameObject s in GameObject.FindGameObjectsWithTag("Structure"))
        //{
        //    Destroy(s);
        //}

        //structures.Clear();

        //Random.InitState(seed);

        using (SeededRNG.Block(seed))
        {
            GenCoastals();
            GenInland();
        }

        //Random.state = oldState;
        //}
    }

    /*
    IEnumerator GenerateOverTime(int seed)
    {
        if (holder != null)
        {
            Destroy(holder.gameObject);
        }

        yield return null;

        holder = new GameObject("POI Holder").transform;
        holder.SetParent(transform);
        holder.localPosition = Vector3.zero;
        holder.localRotation = Quaternion.identity;
        holder.localScale = Vector3.one;

        //Debug.Log("Attempts left: " + attempts);

        //foreach (GameObject s in structures)
        //    if (s != null)
        //        if (Application.isPlaying)
        //            Destroy(s);
        //        else
        //            DestroyImmediate(s);

        //foreach (GameObject s in GameObject.FindGameObjectsWithTag("Structure"))
        //{
        //    Destroy(s);
        //}

        //structures.Clear();

        //Random.InitState(seed);

        using (SeededRNG.Block(seed))
        {
            GenCoastals();
            GenInland();
        }
    }
    */

    void GenCoastals()
    {
        int attempts = 1000;
        List<Vector3> positions = new List<Vector3>(coastalStructures);
        List<Vector3> directions = new List<Vector3>(coastalStructures);

        for (int i = 0; i < coastalStructures && attempts > 0; i++, attempts--)
        {
            Vector3 pt = Random.insideUnitCircle.normalized;
            if (AngleCloseTo(pt, directions, minCoastalStructureAngle))
            {
                i--;
                continue;
            }
            Vector3 angle = pt;

            pt = pt.WithZ(pt.y).WithY(0);
            pt *= radius * 1.5f;
            pt += Vector3.up * seaLevel;
            if (!Physics.Raycast(pt, pt.DirectionTo(Vector3.up * seaLevel), out RaycastHit hit, float.PositiveInfinity, layerMask) || CloseTo(hit.point, positions, minStructureDist))
            {
                i--;
                continue;
            }

            //Debug.Log("Gen coastal " + i);
            positions.Add(hit.point);
            directions.Add(angle);
            Spawn(coastalStructurePrefabs[Random.Range(0, coastalStructurePrefabs.Length)], hit);
            //structures.Add(Instantiate(coastalStructure, hit.point, Quaternion.identity));
        }
    }

    void GenInland()
    {
        int attempts = 1000;
        List<Vector3> positions = new List<Vector3>(inlandStructures);

        for (int i = 0; i < inlandStructures && attempts > 0; i++, attempts--)
        {
            Vector3 pt = Random.insideUnitSphere.Flattened() * radius;
            pt.y = 1000;
            if (!Physics.SphereCast(pt, sphereCastRadius, Vector3.down, out RaycastHit hit, float.PositiveInfinity, layerMask) || hit.point.y > maxHeight || hit.point.y < seaLevel || CloseTo(hit.point, positions, minStructureDist))
            {
                i--;
                continue;
            }

            //Debug.Log("Gen inland " + i);
            positions.Add(hit.point);
            Spawn(inlandStructurePrefabs[Random.Range(0, inlandStructurePrefabs.Length)], hit);
            //structures.Add(Instantiate(inlandStructure, hit.point, Quaternion.identity));
        }
    }

    void Spawn(GameObject prefab, RaycastHit hit)
    {
        Vector3 slopeDir = hit.normal.Flattened().normalized;
        Instantiate(prefab, hit.point, Quaternion.LookRotation(slopeDir), holder);
    }

    static bool CloseTo(Vector3 pos, List<Vector3> positions, float minDist)
    {
        float sqr = minDist * minDist;

        foreach (Vector3 check in positions)
            if (pos.SqrDistance(check) < sqr) return true;

        return false;
    }

    static bool AngleCloseTo(Vector3 angle, List<Vector3> angles, float minAngle)
    {
        foreach (Vector3 check in angles)
            if (Vector3.Angle(angle, check) < minAngle) return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        if (gizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.up * maxHeight, new Vector3(2000, 1, 2000));
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(Vector3.up * seaLevel, new Vector3(2000, 1, 2000));
            Gizmos.DrawWireSphere(Vector3.up * seaLevel, radius);
        }
    }
}
