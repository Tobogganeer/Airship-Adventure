using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
[CustomEditor(typeof(DockSegment))]
public class DockSegmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DockSegment dock = (DockSegment)target;

        for (int i = dock.nodes.Count; i > 0;)
        {
            i--;
            if (dock.nodes[i] == null)
                dock.nodes.RemoveAt(i);
        }

        if (GUILayout.Button("Add Segment"))
        {
            if (dock.nodes.Count == 0)
            {
                GameObject obj = new GameObject("Node 0");
                obj.transform.SetParent(dock.transform, false);
                obj.transform.position = dock.transform.position;
                dock.nodes.Add(obj.transform);
            }
            else if (dock.nodes.Count == 1)
            {
                GameObject obj = new GameObject("Node 1");
                obj.transform.SetParent(dock.transform, false);
                obj.transform.position = dock.nodes[0].position + Vector3.right * 3f;
                dock.nodes.Add(obj.transform);
            }
            else
            {
                int last = dock.nodes.Count - 1;
                Vector3 dir = dock.nodes[last - 1].position.DirectionTo(dock.nodes[last].position);
                dir = dir.Flattened().normalized * 5;

                GameObject obj = new GameObject("Node " + dock.nodes.Count);
                obj.transform.SetParent(dock.transform, false);
                obj.transform.position = dock.nodes[last].position + dir;
                dock.nodes.Add(obj.transform);
            }
        }
    }
}
*/