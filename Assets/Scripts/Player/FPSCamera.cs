using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public static FPSCamera instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform playerBody;
    public Transform lookTransform;

    [Space]
    public Transform vertMoveTransform;
    public Transform vertMoveWeaponTransform;

    private float yRotation;

    //public float sensitivity = 3;
    public float maxVerticalRotation = 90;

    private const float SENSITIVITY_MULT = 0.1f;//3 / 50;
    //                           Default sens / Good cam sens

    private float sensitivity => CurrentSensFromSettings * SENSITIVITY_MULT;

    public static float CurrentSensFromSettings = 50;

    public static float VerticalDip = 0f;

    private const float CrouchOffset = 1f;
    private const float EyeHeight = 0.8f;
    private const float VertDipSmoothing = 4;
    private const float VertDipSpeed = 6;
    private const float VertDipRotationMult = 6f;

    [Space]
    public float sprintHorShake = 0.18f;
    public float sprintVertShake = 0.2f;
    public float sprintRunMul = 4;
    public Transform sprintTransform;


    public static bool Crouched;

    public static Vector3 ViewDir => instance.transform.forward;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //    Shake(Input.GetKey(Inputs.ADS) ? debugAimedShake : debugShake, Input.GetKey(Inputs.ADS) ? debugAimedWeaponShake : debugWeaponShake);

        MouseLook();

        VerticalMovement();
        SprintShake();
    }

    private void MouseLook()
    {
        float x = Input.GetAxisRaw("Mouse X");
        float y = Input.GetAxisRaw("Mouse Y");

        playerBody.Rotate(Vector3.up * x * sensitivity);
        // Rotates the body horizontally

        yRotation = Mathf.Clamp(yRotation - y * sensitivity, -maxVerticalRotation, maxVerticalRotation);
        //float clampedRotWithRecoil = yRotation;
        float clampedRotWithRecoil = Mathf.Clamp(yRotation, -maxVerticalRotation, maxVerticalRotation);

        // Clamps the Y rotation so you can only look straight up or down, not backwards
        lookTransform.localRotation = Quaternion.Euler(new Vector3(clampedRotWithRecoil, 0));
    }

    private void VerticalMovement()
    {
        float crouchOffset = Crouched ? CrouchOffset : 0f;
        vertMoveTransform.localPosition = Vector3.Lerp(vertMoveTransform.localPosition, Vector3.down * (VerticalDip/* - EyeHeight*/ + crouchOffset), Time.deltaTime * VertDipSmoothing);
        vertMoveTransform.localRotation = Quaternion.Slerp(vertMoveTransform.localRotation, Quaternion.Euler(VerticalDip * VertDipRotationMult, 0, 0), Time.deltaTime * VertDipSmoothing);
        //vertMoveWeaponTransform

        //VerticalDip = Mathf.MoveTowards(VerticalDip, 0, Time.deltaTime * VertDipSpeed);
        VerticalDip = Mathf.Lerp(VerticalDip, 0, Time.deltaTime * VertDipSpeed);
    }

    private void SprintShake()
    {
        if (!PlayerMovement.Grounded) return;

        float sin = WeaponSway.SinValue;
        float vert = sin * sin * sin;
        Vector3 desired = new Vector3(Mathf.Abs(vert) * sprintVertShake, -sin * sprintHorShake);
        //Vector3 desired = Vector3.zero;

        sprintTransform.localRotation = Quaternion.Slerp(sprintTransform.localRotation, Quaternion.Euler(desired), Time.deltaTime * 10);
    }
}