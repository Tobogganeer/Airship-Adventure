using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    public Transform explodeFrom;
    public float force = 150f;
    public float radius = 35f;
    public float upwardsModifier = 1.5f;

    void Start()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(force, explodeFrom.position, radius, upwardsModifier, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if (explodeFrom != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(explodeFrom.position, radius);
        }
    }
}
