using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSizer : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public float width = 0.1f;

    [Space]
    public Mesh cubeMesh;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (start != null && end != null && cubeMesh != null)
        {
            Vector3 pos = Vector3.Lerp(start.position, end.position, 0.5f);
            Quaternion rot = Quaternion.LookRotation(start.position.DirectionTo(end.position));
            Vector3 scale = new Vector3(width, width, start.position.Distance(end.position));
            Gizmos.DrawMesh(cubeMesh, pos, rot, scale);
            Gizmos.DrawWireSphere(start.position, 0.1f);
            Gizmos.DrawWireSphere(end.position, 0.1f);
        }
    }

    [ContextMenu("Size")]
    public void Size()
    {
        transform.position = Vector3.Lerp(start.position, end.position, 0.5f);
        transform.rotation = Quaternion.LookRotation(start.position.DirectionTo(end.position));
        transform.localScale = new Vector3(width, width, start.position.Distance(end.position));
    }
}
