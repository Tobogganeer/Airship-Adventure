using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTesting : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        float dot = Vector3.Dot(transform.forward,transform.position.Flattened()
            .DirectionTo(target.position.Flattened()));
        float val;
        if (dot > 0)
            val = Remap.Float(dot, 1f, 0f, 0f, 0.5f);
        else
            val = Remap.Float(dot, 0f, -1f, 0.5f, 1f);
        Debug.Log(val);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
    }
}
