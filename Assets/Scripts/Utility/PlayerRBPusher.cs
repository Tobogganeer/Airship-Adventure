using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRBPusher : MonoBehaviour
{
    public float force = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            Vector3 pt = other.ClosestPoint(transform.position);
            rb.AddForceAtPosition(transform.position.DirectionTo(pt) * force, pt);
        }
    }
}
