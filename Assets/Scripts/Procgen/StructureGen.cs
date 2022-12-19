using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGen : MonoBehaviour
{
    [Range(0, 10)] public int coastalStructures = 3;
    [Range(0, 15)] public int inlandStructures = 5;

    [Space]
    public float maxHeight = 100;
    public float seaLevel = 50;
    public float radius = 2500;

    [Space]
    public GameObject coastalStructure;
    public GameObject inlandStructure;


    [Space]
    public bool gizmos;

    [SerializeField, HideInInspector]
    List<GameObject> structures = new List<GameObject>();

    public void Generate(int seed)
    {
        //Random.State oldState = Random.state;

        foreach (GameObject s in structures)
            if (s != null)
                if (Application.isPlaying)
                    Destroy(s);
                else
                    DestroyImmediate(s);

        //foreach (GameObject s in GameObject.FindGameObjectsWithTag("Structure"))
        //{
        //    Destroy(s);
        //}

        structures.Clear();

        //Random.InitState(seed);

        using (SeededRNG.Block(seed))
        {
            GenCoastals();
            GenInland();
        }

        //Random.state = oldState;
    }

    void GenCoastals()
    {
        int attempts = 100;

        for (int i = 0; i < coastalStructures && attempts > 0; i++, attempts--)
        {
            Vector3 pt = Random.insideUnitCircle.normalized;
            pt = pt.WithZ(pt.y).WithY(0);
            pt *= radius * 1.5f;
            pt += Vector3.up * seaLevel;
            if (!Physics.Raycast(pt, pt.DirectionTo(Vector3.up * seaLevel), out RaycastHit hit))
            {
                i--;
                continue;
            }

            structures.Add(Instantiate(coastalStructure, hit.point, Quaternion.identity));
        }
    }

    void GenInland()
    {
        int attempts = 10000;

        for (int i = 0; i < inlandStructures && attempts > 0; i++, attempts--)
        {
            Vector3 pt = Random.insideUnitSphere.Flattened() * radius;
            pt.y = 1000;
            if (!Physics.Raycast(pt, Vector3.down, out RaycastHit hit) || hit.point.y > maxHeight || hit.point.y < seaLevel)
            {
                i--;
                continue;
            }

            structures.Add(Instantiate(inlandStructure, hit.point, Quaternion.identity));
        }
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
