using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//https://answers.unity.com/questions/1358491/character-controller-slide-down-slope.html

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    private void Awake()
    {
        instance = this;
    }

    private CharacterController controller;
    //public Transform airship;

    [Space]
    public float moveSpeed = 3.5f;
    public float slopeLimit = 60f;
    public float gravity = 10f;
    public float slideFriction = 0.5f;
    public float slopeAccelPercent = 0.5f;
    public float groundAcceleration = 8f;
    public float airAcceleration = 1f;
    public float accelLerpSpeed = 5f;
    public float pushPower = 3f;

    [Space]
    public float groundTimeToWalkOnSlopes = 0.5f;
    public float slopePushPower = 0.3f;

    public LayerMask groundLayerMask;

    private Vector3 groundNormal;
    private bool grounded;
    private bool wasGrounded;
    private bool groundNear;

    private float y;

    private float timeOnSlope;
    private float timeOnGround;

    private float groundAngle;

    const float DOWNFORCE = 3f;
    //float slopeMult;

    Vector3 desiredVelocity;
    Vector3 moveVelocity;
    Vector3 actualVelocity;
    Vector3 lastPos;
    float slopeTime;

    #region Constants

    //Crouch
    //const float CrouchRaySize = 0.4f;
    //const float CrouchRayLength = 1f;
    //const float StandingHeight = 2f;
    //const float CrouchingHeight = 1f;
    //const float CrouchHeightDif = StandingHeight - CrouchingHeight;

    //Grounded
    const float GroundedSphereRadius = 0.475f;
    const float GroundedSphereDist = 0.7f;
    const float GroundNearDist = 1.8f;
    const float NearSurfaceDist = 0.8f;
    const float NearSurfaceRadius = 0.55f;
    const float GroundedRayDist = 1.2f; // backup for sphere


    // Other
    const float SlideSpeedDecreaseMult = 2.65f;
    const float SlideMoveDirInfluence = 0.6f;

    #endregion

    private float cur_speed;
    private float cur_accel;

    private float airtime;

    public static event Action<float> OnLand;

    //Vector3 lastAirshipPos;
    //Vector3 lastAirshipRot;

    //private bool slidingFromSpeed;

    public static bool Grounded => instance.grounded;
    public static bool Moving { get; private set; }
    public static bool Sliding { get; private set; }
    public static Vector3 Position => instance.transform.position;

    public static Vector3 LocalVelocity { get; private set; }
    public static Vector3 WorldVelocity { get; private set; }

    public static float NormalizedSpeed { get; private set; }
    public static float AirTime { get; private set; }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = 80;
        lastPos = transform.position;
        slopeTime = 1;
        groundNormal = Vector3.up;

        //lastAirshipPos = airship.position;
        //lastAirshipRot = airship.eulerAngles;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //groundNormal = hit.normal;
        /*
        var body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
            body.velocity += controller.velocity;
        */
        Rigidbody body = hit.collider.attachedRigidbody;
        Vector3 force;

        if (body == null || body.isKinematic) return;

        if (hit.moveDirection.y < -0.3f)
            force = Vector3.down * gravity;
        else
            force = hit.controller.velocity * pushPower;

        // Apply the push
        body.AddForceAtPosition(force, hit.point);
    }


    private void Update()
    {
        cur_speed = moveSpeed; // UpdateSpeed();
        UpdateAcceleration();

        Move();

        UpdateGrounded();

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //    Time.timeScale = Time.timeScale < 0.3f ? 1f : 0.25f;

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //    FPSCamera.VerticalDip += 1f;

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //    AddImpulse(FPSCamera.instance.transform.forward);

        //actualVelocity = (transform.position - lastPos) / Time.deltaTime;
        //lastPos = transform.position;
        //SetProperties();
        // Moved to late update ^^^

        actualVelocity = moveVelocity;
        lastPos = transform.position;

        SetProperties();
    }

    private void SetProperties()
    {
        WorldVelocity = actualVelocity;
        LocalVelocity = transform.InverseTransformVector(WorldVelocity);
        Moving = desiredVelocity.sqrMagnitude > 0.1f && WorldVelocity.Flattened().sqrMagnitude > 0.1f;
        Sliding = Grounded && /*Crouched &&*/ (timeOnSlope > 0.3f);
        NormalizedSpeed = 1;
        AirTime = airtime;
    }

    private void Move()
    {
        const float SlopeSpeedIncrease = 5;
        const float MaxSlopeTime = SlopeSpeedIncrease * 4.5f;

        Vector2 input = PlayerInputs.Movement;

        if (Interactor.Interacting)
        {
            desiredVelocity = transform.position.DirectionTo_NoNormalize(Interactor.CurrentInteractable.InteractFrom.position).Flattened();
        }
        else
        {
            desiredVelocity = transform.right * input.x + transform.forward * input.y;
        }

        desiredVelocity *= cur_speed;

        y -= gravity * Time.deltaTime;

        if (grounded)
        {
            y = -DOWNFORCE;

            if (!wasGrounded)
            {
                // Just landed
                OnLand?.Invoke(airtime);
                FPSCamera.VerticalDip += Mathf.Lerp(0.0f, 2f, airtime * 0.6f);

                airtime = 0;
            }
        }
        else airtime += Time.deltaTime;


        // On a slope, but not above a void or something (will fall onto ground)
        if (timeOnSlope > 0.2f)// && groundNear) // Commented out to try to counteract walking up edges of slopes
        {
            //if (groundNear)
                SlopeMovement();
            //else
            //    AirSlopeMovement();
        }
        // Let player still move sideways on slopes

        if (timeOnSlope > 0.2f)
        {
            // On slope and hasnt like just jumped

            //moveDir = Vector3.zero;
            Vector3 slopeHorDir = groundNormal.Flattened().normalized;

            if (groundNear)
            {
                Vector3 velDir = actualVelocity.Flattened().normalized;
                float velSimilarity = Vector3.Dot(slopeHorDir, velDir);

                if (velSimilarity > -0.1f)
                    slopeTime += Time.deltaTime * SlopeSpeedIncrease;
                else
                    slopeTime -= Time.deltaTime * SlopeSpeedIncrease;

                if (slopeTime > MaxSlopeTime)
                    slopeTime = MaxSlopeTime;

                if (slopeTime < 1f)
                    slopeTime = 1f;

                y = -DOWNFORCE * slopeTime;
            }
            else
                slopeTime = 1f;

            desiredVelocity.x += groundNormal.x * slopeTime * (1f - slideFriction);
            desiredVelocity.z += groundNormal.z * slopeTime * (1f - slideFriction);

            float angle = Vector3.Angle(Vector3.up, groundNormal);
            if (angle > slopeLimit)
            {
                const float ViewInfluence = 1.25f;
                const float MaxViewInfluence = 2.5f;
                const float Mult = 0.2f;

                Vector3 viewDir = FPSCamera.ViewDir.Flattened().normalized;
                Vector3 slopeSide = Vector3.Cross(slopeHorDir, Vector3.up);
                float viewSimilarity = Vector3.Dot(slopeSide, viewDir);

                float mul = ViewInfluence * Mathf.Abs(viewSimilarity) * slopeTime * Mult;
                mul = Mathf.Clamp(mul, 0, MaxViewInfluence);

                desiredVelocity.x += viewDir.x * mul;
                desiredVelocity.z += viewDir.z * mul;
            }
        }
        else
        {
            slopeTime = 1f;
        }

        if (wasGrounded && !grounded)
        {
            // Left ground (not from a jump, otherwise why cancel y velocity)
            //y += DOWNFORCE; // counteract downforce, set y to 0
            y = 0; // didn't work as downforce was set multiplied with downforce earlier, just set to 0
        }

        //moveDir.y = y;

        //Vector3 flatVel = actualVelocity.Flattened();
        Vector3 flatVel = moveVelocity.Flattened(); // AIRSHIP MOVEMENT -----

        //if (actualVelocity.y > 0)
        //y = actualVelocity.y;
        //this.moveVelocity.y = actualVelocity.y;

        //moveVelocity = Vector3.Lerp(flatVel, desiredVelocity, Time.deltaTime * cur_accel).WithY(y);
        moveVelocity = Vector3.Lerp(flatVel, desiredVelocity, Time.deltaTime * cur_accel).WithY(0);

        moveVelocity.y = y;

        controller.Move(moveVelocity * Time.deltaTime);

    }

    private void SlopeMovement()
    {
        // Normal slope movement
        //Vector3 slopeHorDir = groundNormal.Flattened().normalized;
        //Vector3 slopeSide = Vector3.Cross(slopeHorDir, Vector3.up);
        //Vector3 velDir = desiredVelocity.Flattened().normalized;

        //float similarity = Vector3.Dot(slopeSide, velDir);
        //similarity *= similarity;

        //const float MaxControl = 0.7f;

        //float control = Mathf.Clamp(Mathf.Abs(similarity) * slopeAccelPercent * slopeTime, 0.1f, MaxControl);
        //desiredVelocity *= control;
        Vector3 slopeHorDir = groundNormal.WithY(-groundNormal.y);
        desiredVelocity = slopeHorDir;
    }

    private void AirSlopeMovement()
    {
        // Slope movement with no ground beneath, like say walking on a fence top
        Vector3 slopeHorDir = groundNormal.Flattened().normalized;
        // ^^^ points directly towards edge
        Vector3 velDir = desiredVelocity.Flattened().normalized;

        float similarity = Vector3.Dot(slopeHorDir, velDir);
        //similarity *= similarity;

        const float MaxControl = 0.9f;

        float control = Mathf.Clamp(Mathf.Abs(similarity) * slopeAccelPercent * slopeTime, 0.1f, MaxControl);
        desiredVelocity *= control;
    }

    private void UpdateAcceleration()
    {
        float target = grounded ? groundAcceleration : airAcceleration;

        cur_accel = Mathf.Lerp(cur_accel, target, Time.deltaTime * accelLerpSpeed);
    }

    private void UpdateGrounded()
    {
        groundAngle = Vector3.Angle(Vector3.up, groundNormal);

        if (groundAngle > slopeLimit)
        {
            timeOnSlope += Time.deltaTime;
            timeOnGround = 0f;
        }
        else
        {
            timeOnSlope = 0f;
            timeOnGround += Time.deltaTime;
        }

        if (timeOnGround < groundTimeToWalkOnSlopes)
        {
            controller.slopeLimit = slopeLimit;
        }
        else
        {
            controller.slopeLimit = 80f;
        }

        wasGrounded = grounded;
        RaycastHit hit;
        grounded = Physics.SphereCast(new Ray(transform.position, Vector3.down), GroundedSphereRadius, out hit, GroundedSphereDist, groundLayerMask)
            || Physics.Raycast(transform.position, Vector3.down, out hit, GroundedRayDist, groundLayerMask);

        if (grounded)
            groundNormal = hit.normal;

        groundNear = Physics.Raycast(new Ray(transform.position, Vector3.down), GroundNearDist, groundLayerMask);

        if (!Physics.CheckSphere(transform.position + Vector3.down * NearSurfaceDist, NearSurfaceRadius, groundLayerMask))
            groundNormal = Vector3.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.down * GroundedSphereDist, GroundedSphereRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * GroundedRayDist);
        // Grounded

        if (controller == null)
            controller = GetComponent<CharacterController>();
    }


    private void LateUpdate()
    {
        /*
        Vector3 airshipVel = airship.position - lastAirshipPos;
        Vector3 airshipRot = airship.eulerAngles - lastAirshipRot;

        //Vector3 diff = transform.position - lastPos;

        //transform.Translate(airshipVel, Space.World);
        controller.enabled = false;
        transform.RotateAround(airship.position, Vector3.up, airshipRot.y);
        controller.enabled = true;
        controller.Move(airshipVel);

        //Vector3 playerTranslation = transform.position - pos;
        //lastPos -= playerTranslation;
        //lastPos = transform.position + diff;

        lastAirshipPos = airship.position;
        lastAirshipRot = airship.eulerAngles;
        

        //actualVelocity = (transform.position - lastPos) / Time.deltaTime; // AIRSHIP MOVEMENT -----
        actualVelocity = moveVelocity;
        lastPos = transform.position;

        SetProperties();
        */
    }
}