using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour, IInteractable
{
    public LineRenderer laser;
    public LineRenderer rope;
    public Transform hook;
    public Transform head;

    [field: SerializeField]
    public Transform InteractFrom { get; set; }
    public bool IsInteracting { get; set; }

    [Space]
    public float turnSpeed = 5f;
    public LayerMask targetMask;
    public Vector3 defaultRot = Vector3.up * 180;

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
    }

    private void LateUpdate()
    {
        if (IsInteracting)
        {
            Quaternion cam = Quaternion.LookRotation(FPSCamera.ViewDir);
            head.localRotation = Quaternion.Slerp(head.localRotation, cam, turnSpeed * Time.deltaTime);
        }
        else
        {
            head.localRotation = Quaternion.Slerp(head.localRotation,
                Quaternion.Euler(defaultRot), turnSpeed * Time.deltaTime);
        }

        rope.SetPosition(0, rope.transform.position);
        rope.SetPosition(1, hook.position);

        laser.SetPosition(0, laser.transform.position);
        laser.SetPosition(1, head.forward * 1000 + laser.transform.position);
    }
}
