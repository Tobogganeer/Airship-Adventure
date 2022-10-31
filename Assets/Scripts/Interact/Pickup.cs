using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    bool IInteractable.FixedPosition => false;
    public bool IsInteracting { get; set; }
    Transform IInteractable.InteractFrom => throw new System.NotImplementedException();
    Rigidbody rb;
    bool blasto;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
        blasto = IsInteracting == false;
    }

    private void FixedUpdate()
    {
        // https://github.com/samhogan/PhysicsPickup-Unity/blob/master/PhysicsPickup.cs

        //if (blasto)
        //    rb.AddTorque(Random.rotation.eulerAngles * 0.1f, ForceMode.Impulse);

        if (IsInteracting)
        {
            rb.useGravity = false;
            Vector3 targetPos = Interactor.InteractFrom.position + Interactor.InteractFrom.forward * Interactor.InteractRange;
            Quaternion targetRot = Interactor.InteractFrom.rotation;// * relRot;

            //interpolate to the target position using velocity
            rb.velocity = (targetPos - rb.position) * 10f;

            //keep the relative rotation the same
            //rb.rotation = targetRot;
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRot, Time.deltaTime * 10f);

            //no spinning around
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.useGravity = true;
        }
    }
}
