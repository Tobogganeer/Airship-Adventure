using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour, IInteractable
{
    public GrappleHook other;
    public LineRenderer laser;
    public LineRenderer rope;
    public Transform hook;
    public Transform hookHome;
    public Transform head;

    [field: SerializeField]
    public Transform InteractFrom { get; set; }
    public bool IsInteracting { get; set; }

    [Space]
    public float turnSpeed = 5f;
    public LayerMask targetMask;
    //public Vector3 defaultRot = Vector3.up * 180;
    public float defaultRot = 180f;

    public Material red;
    public Material green;

    public Transform grabbedTarget { get; private set; }
    Vector3 grabbedPos;
    bool grabbing;

    //readonly WaitForSeconds wait = new WaitForSeconds(1f);
    //readonly float wait = 1f;
    readonly float travel = 2f;

    public void OnInteract()
    {
        if (grabbing) return;
        IsInteracting = !IsInteracting;
    }

    private void LateUpdate()
    {
        if (grabbing && grabbedTarget != null)
        {
            Quaternion cam = Quaternion.LookRotation(head.position.DirectionTo(grabbedTarget.position + grabbedPos));
            head.rotation = Quaternion.Slerp(head.rotation, cam, turnSpeed * Time.deltaTime);
            laser.enabled = false;
        }
        else if (IsInteracting)
        {
            Quaternion cam = Quaternion.LookRotation(FPSCamera.ViewDir);
            head.rotation = Quaternion.Slerp(head.rotation, cam, turnSpeed * Time.deltaTime);
            laser.enabled = true;
            laser.SetPosition(0, laser.transform.position);
            laser.SetPosition(1, head.forward * 1000 + laser.transform.position);
            TryShoot();
        }
        else
        {
            head.localRotation = Quaternion.Slerp(head.localRotation,
                Quaternion.Euler(0, defaultRot, 0), turnSpeed * Time.deltaTime);
            laser.enabled = false;
        }

        rope.SetPosition(0, rope.transform.position);
        rope.SetPosition(1, hook.position);
    }

    void TryShoot()
    {
        if (grabbing)
        {
            laser.sharedMaterial = green;
            return;
        }

        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, Mathf.Infinity, targetMask) && hit.transform != other.grabbedTarget)
        {
            laser.sharedMaterial = green;

            if (PlayerInputs.Primary && !grabbing)
            {
                grabbedTarget = hit.transform;
                grabbedPos = hit.transform.position.DirectionTo_NoNormalize(hit.point);
                StartCoroutine(GrabTarget());
                AudioManager.Play(new Audio("GrappleLaunch").SetPosition(transform.position).SetParent(transform));
            }
        }
        else
        {
            laser.sharedMaterial = red;
        }
    }

    IEnumerator GrabTarget()
    {
        grabbing = true;
        IsInteracting = false;
        float time = 0f;

        while (time < travel)
        {
            time += Time.deltaTime;

            if (grabbedTarget != null)
            {
                hook.transform.position = Vector3.Lerp(hookHome.position,
                    grabbedTarget.position + grabbedPos, Remap.Float(time, 0, travel, 0, 1));
            }
            yield return null;
        }

        //yield return wait;


        Vector3 hookOffset = hook.position.DirectionTo_NoNormalize(grabbedTarget != null ? grabbedTarget.position : Vector3.zero);
        Vector3 grab = hook.position;

        while (time > 0)
        {
            time -= Time.deltaTime;
            hook.position = Vector3.Lerp(hookHome.position,
                grab, Remap.Float(time, 0, travel, 0, 1));
            if (grabbedTarget != null)
            {
                grabbedTarget.position = hook.position + hookOffset;
                if (time < 0.2f)
                {
                    grabbedTarget.GetComponent<Cache>().OnReelIn();
                    grabbedTarget = null;
                }
            }
            yield return null;
        }

        grabbing = false;
    }
}
