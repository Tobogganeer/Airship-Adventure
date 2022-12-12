using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour, IInteractable
{
    bool IInteractable.FixedPosition => false;
    public bool IsInteracting { get; set; }
    Transform IInteractable.InteractFrom => throw new System.NotImplementedException();
    Rigidbody rb;
    Spring spring;

    [Header("Carrying")]
    public float carryForce = 10f;
    public bool useMass = false;
    public float carryRange = 2;

    [Header("Spawning")]
    public bool useSpring = true;
    public float springStrength = 50f;
    public float springVelocity = 10f;
    public float springDamper = 6f;
    private float timerspring = 3f;
    Vector3 scale;
    //bool blasto;

    const float MaxRange = 6f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (useSpring)
        {
            spring = new Spring();
            spring.SetTarget(1f);
            spring.SetDamper(springDamper);
            spring.SetStrength(springStrength);
            spring.SetVelocity(springVelocity);
            scale = transform.localScale;
            transform.localScale = spring.Value * scale;
        }
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
        //blasto = IsInteracting == false;
    }

    private void FixedUpdate()
    {
        // https://github.com/samhogan/PhysicsPickup-Unity/blob/master/PhysicsPickup.cs

        //if (blasto)
        //    rb.AddTorque(Random.rotation.eulerAngles * 0.1f, ForceMode.Impulse);

        if (IsInteracting)
        {
            rb.useGravity = false;
            Vector3 targetPos = Interactor.InteractFrom.position + Interactor.InteractFrom.forward * carryRange;
            Quaternion targetRot = Interactor.InteractFrom.rotation;// * relRot;

            //interpolate to the target position using velocity
            float mass = useMass ? rb.mass : 1f;
            rb.velocity = (targetPos - rb.position) * (carryForce / mass);

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

    private void Update()
    {
        if (useSpring)
        {
            spring.Update(Time.deltaTime);
            transform.localScale = spring.Value * scale;
            timerspring -= Time.deltaTime;
            if ( timerspring <= 0)
            {
                transform.localScale = scale;
                useSpring = false;
            }
        }

        if (transform.position.SqrDistance(Interactor.InteractFrom.position) > MaxRange * MaxRange)
            IsInteracting = false;

        /*
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            spring.SetTarget(1f);
            spring.SetDamper(springDamper);
            spring.SetStrength(springStrength);
            spring.SetVelocity(springVelocity);
            spring.Reset();
        }
        */
    }

    private void OnDestroy()
    {
        Interactor.OnDestroy(this);
    }
}
