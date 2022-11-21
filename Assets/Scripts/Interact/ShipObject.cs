using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipObject : MonoBehaviour
{
    public float teleportDistance = 30f;

    private void Update()
    {
        if (transform.position.SqrDistance(Airship.instance.
            transform.position) > teleportDistance * teleportDistance)
        {
            transform.position = Airship.instance.spawnCrapHere.position;
            if (!Airship.instance.kiddos.Contains(transform))
                Airship.instance.kiddos.Add(transform);
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
