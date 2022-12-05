using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockSegment : MonoBehaviour
{
    public Vector3 plankSize = new Vector3(2, 0.15f, 0.5f); // Default plank size
    public float spacing = 0.1f;
    public int postSpacing = 3;
    public float postOffsetToEdge = 0.2f;

    //public Transform start;
    //public Transform end;
    //public List<Transform> nodes = new List<Transform>();

    [Space]
    public bool drawGizmos = true;
    public Mesh plankMesh;
    public Mesh pillarMesh;
    public Mesh intersectionMesh;

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        int children = transform.childCount;

        if (children == 0) return;

        Gizmos.color = Color.white;

        List<Transform> all = new List<Transform>();
        AddAllChildren(transform, all);

        foreach (Transform node in all)
        {
            //if (node == null) return;
            if (intersectionMesh)
                Gizmos.DrawWireMesh(intersectionMesh, node.position, Quaternion.identity);
            else
                Gizmos.DrawWireSphere(node.position, 0.1f);

            //DrawNodeTree(node);
        }

        //if (nodes.Count < 2) return;

        Gizmos.color = Color.yellow;

        DrawNodeTree(transform, false);

        //for (int i = 0; i < nodes.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
        //    DrawNodePair(nodes[i], nodes[i + 1]);
        //}
    }

    void AddAllChildren(Transform root, List<Transform> list)
    {
        foreach (Transform child in root)
        {
            list.Add(child);
            AddAllChildren(child, list);
        }
    }

    void DrawNodeTree(Transform parent, bool includeParent = true)
    {
        if (parent.childCount == 0) return;

        DrawNodeTree(parent.GetChild(0));
        if (includeParent)
        {
            Gizmos.DrawLine(parent.position, parent.GetChild(0).position);
            DrawNodePair(parent, parent.GetChild(0));
        }

        if (parent.childCount == 1) return;

        for (int i = 0; i < parent.childCount - 1; i++)
        {
            DrawNodeTree(parent.GetChild(i + 1));
            DrawNodePair(parent.GetChild(i), parent.GetChild(i + 1));
        }

        //for (int i = 0; i < nodes.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
        //    DrawNodePair(nodes[i], nodes[i + 1]);
        //}
    }

    void DrawNodePair(Transform node1, Transform node2)
    {
        Quaternion rot = Quaternion.LookRotation(node1.position.DirectionTo(node2.position));

        List<Vector3> planks = GetPlankPositions(node1.position, node2.position);
        foreach (Vector3 pos in planks)
        {
            if (plankMesh)
                Gizmos.DrawWireMesh(plankMesh, pos, rot);
            else
                Gizmos.DrawWireSphere(pos, 0.1f);
        }

        List<Vector3> pillars = GetPillarPositions(planks);
        foreach (Vector3 pos in pillars)
        {
            if (pillarMesh)
                Gizmos.DrawWireMesh(pillarMesh, pos, Quaternion.identity);
            else
                Gizmos.DrawWireSphere(pos, 0.1f);
        }
    }

    List<Vector3> GetPlankPositions(Vector3 start, Vector3 end)
    {
        List<Vector3> positions = new List<Vector3>();
        float distance = Vector3.Distance(start, end);

        int planks = Mathf.FloorToInt(distance / (plankSize.z + spacing));

        for (int i = 0; i < planks; i++)
        {
            positions.Add(Vector3.Lerp(start, end, i / (float)planks));
        }
        
        return positions;
    }

    List<Vector3> GetPillarPositions(List<Vector3> plankPositions)
    {
        List<Vector3> positions = new List<Vector3>();
        int numPosts = plankPositions.Count / postSpacing;

        for (int i = 0; i < numPosts; i++)
        {
            int plank = 1 + i * postSpacing;
            Vector3 dir = plankPositions[plank].DirectionTo(plankPositions[plank + 1]);
            Vector3 cross = Vector3.Cross(dir, Vector3.up);
            cross.Normalize();
            Vector3 smallCross = cross * postOffsetToEdge;
            positions.Add(plankPositions[plank] + cross.normalized - smallCross);
            positions.Add(plankPositions[plank] - cross.normalized + smallCross);
        }

        return positions;
    }
}
