using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DotTesting : MonoBehaviour
{
    public Transform target;
    public float val;
    public bool method2;

    void Update()
    {
        if (target == null) return;

        //float dot = Vector3.Dot(transform.forward, transform.position.Flattened()
        //    .DirectionTo(target.position.Flattened()));
        float dot = Vector3.Dot(transform.forward, target.forward);

        val = dot;

        if (method2)
        {
            if (dot > 0)
                val = Remap.Float(dot, 1f, 0f, 0f, 0.5f);
            else
                val = Remap.Float(dot, 0f, -1f, 0.5f, 1f);
        }
        //Debug.Log(val);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);

        if (target == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position, target.position + target.forward * 5);
    }
}
