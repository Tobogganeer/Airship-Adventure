using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPipe : MonoBehaviour
{
    public Transform ramp;
    public float ejectForce = 10;
    public float angularVelocity = 720;
    public Vector3 defaultRot = new Vector3(0, 180, 0);
    public Vector3 ejectRot = new Vector3(-40, 180, 0);
    public float raiseTime = 1f;
    public float raiseSpeed = 135f;
    public float lowerSpeed = 90f;
    float timer;
    Quaternion rot;

    [Space]
    public Mesh rampMesh;

    //public Rigidbody test;

    public void Spawn(Rigidbody rb)
    {
        if (rb.TryGetComponent(out Pickup p))
            p.Spawn();

        timer = raiseTime;
        rb.transform.position = transform.position;
        rb.transform.rotation = Random.rotation;
        rb.velocity = transform.forward * ejectForce;
        rb.angularVelocity = Random.onUnitSphere * Random.Range(-angularVelocity, angularVelocity) * Mathf.Deg2Rad;
    }

    private void Update()
    {
        //if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        //    Spawn(test);

        timer -= Time.deltaTime;

        if (timer > 0)
        {
            //ramp.localRotation = Quaternion.RotateTowards(ramp.localRotation, Quaternion.Euler(ejectRot), Time.deltaTime * raiseSpeed);
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(ejectRot), Time.deltaTime * raiseSpeed);
            ramp.localRotation = Quaternion.Slerp(ramp.localRotation, rot, Time.deltaTime * 5);
        }
        else
        {
            //ramp.localRotation = Quaternion.RotateTowards(ramp.localRotation, Quaternion.Euler(defaultRot), Time.deltaTime * raiseSpeed);
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(defaultRot), Time.deltaTime * lowerSpeed);
            ramp.localRotation = Quaternion.Slerp(ramp.localRotation, rot, Time.deltaTime * 5);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * ejectForce);

        if (rampMesh == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireMesh(rampMesh, ramp.position, ramp.rotation);
        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(rampMesh, ramp.position, ramp.rotation * Quaternion.Euler(ejectRot - ramp.localEulerAngles));
    }
}
