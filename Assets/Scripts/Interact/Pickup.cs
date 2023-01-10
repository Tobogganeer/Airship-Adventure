using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour, IInteractable, ISecondaryInteractable
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
    public float throwForce = 7.5f;

    [Header("Spawning")]
    public float start = 0f;
    public float springStrength = 30f;
    public float springVelocity = 1f;
    public float springDamper = 10f;

    const float DisableTime = 2.0f;
    float timer;
    bool springActive;
    //public Vector3 scale = Vector3.one;
    Vector3 scale;
    //Quaternion rot;
    //Vector3 rot;
    //bool blasto;

    bool _throw;

    const float MaxRange = 6f;

    private void Awake()
    {
        scale = transform.localScale;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //scale = transform.localScale;
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
        if (!IsInteracting)
            _throw = true;
        //rot = transform.rotation;
        //rot = transform.eulerAngles;
        //rot = Quaternion.FromToRotation(Interactor.InteractFrom.forward, transform.forward);
        //blasto = IsInteracting == false;
    }

    public void OnSecondaryInteract()
    {
        if (IsInteracting)
        {
            IsInteracting = false;
        }
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
            Quaternion targetRot = Interactor.InteractFrom.rotation;
            //Quaternion targetRot = rot * Interactor.InteractFrom.rotation;
            //Quaternion targetRot = Quaternion.Euler(Interactor.InteractFrom.eulerAngles + rot);


            //interpolate to the target position using velocity
            float mass = useMass ? rb.mass : 1f;
            rb.velocity = (targetPos - rb.position) * (carryForce / mass);

            //keep the relative rotation the same
            //rb.rotation = targetRot;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            //transform.rotation = targetRot;

            //no spinning around
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.useGravity = true;
            if (_throw)
            {
                _throw = false;
                rb.AddForce(Interactor.InteractFrom.forward * throwForce, ForceMode.VelocityChange);
            }
        }
    }

    private void Update()
    {
        //if (Keyboard.current.spaceKey.wasPressedThisFrame)
        //    Spawn();

        if (springActive)
        {
            spring.Update(Time.deltaTime);
            transform.localScale = spring.Value * scale;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                transform.localScale = scale;
                springActive = false;
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

    public void Spawn()
    {
        springActive = true;

        timer = DisableTime;
        spring = new Spring();
        spring.SetTarget(1f);
        spring.SetDamper(springDamper);
        spring.SetStrength(springStrength);
        spring.SetVelocity(springVelocity);
        spring.SetValue(start);
        //transform.localScale = spring.Value * scale;
        transform.localScale = scale * start;
    }
}
