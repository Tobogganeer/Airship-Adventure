using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCamera : MonoBehaviour
{
    public static FPSCamera instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform playerBody;
    public Transform lookTransform;
    float eyeHeight;

    //[Space]
    //public Transform vertMoveTransform;
    //public Transform vertMoveWeaponTransform;

    private float yRotation;
    public static float YRot => instance.yRotation;

    //public float sensitivity = 3;
    public float maxVerticalRotation = 90;

    private const float SENSITIVITY_MULT = 0.1f;//3 / 50;
    //                           Default sens / Good cam sens

    private float sensitivity => SettingsSensitivity * SENSITIVITY_MULT;

    public static float SettingsSensitivity = 35f;
    public static float SettingsFOV = 60f;

    public static float VerticalDip = 0f;

    private const float VertDipSmoothing = 4f;
    private const float VertDipSpeed = 6f;
    private const float VertDipRotationMult = 6f;

    /*
    [Space]
    public float sprintHorShake = 0.18f;
    public float sprintVertShake = 0.2f;
    public float sprintRunMul = 4;
    
    public static bool Crouched;
    */

    public static Transform Transform => instance.transform;
    public static Vector3 Position => instance.transform.forward;
    public static Vector3 ViewDir => instance.transform.forward;

    Vector3 pos;
    Quaternion rot;
    Quaternion sprintRot;

    private void Start()
    {
        eyeHeight = lookTransform.localPosition.y;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        GetComponent<Camera>().fieldOfView = SettingsFOV;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //    Shake(Input.GetKey(Inputs.ADS) ? debugAimedShake : debugShake, Input.GetKey(Inputs.ADS) ? debugAimedWeaponShake : debugWeaponShake);

        SensControls();
        MouseLook();

        VerticalMovement();
        //SprintShake();

        lookTransform.localPosition = pos + Vector3.up * eyeHeight;
        lookTransform.localRotation = rot * sprintRot;
    }

    private void SensControls()
    {
        if (Keyboard.current.equalsKey.wasPressedThisFrame)
        {
            SettingsSensitivity = Mathf.Min(SettingsSensitivity += 5f, 75f);
            PopUp.Show("Current sens: " + SettingsSensitivity, 1f);
        }
        if (Keyboard.current.minusKey.wasPressedThisFrame)
        {
            SettingsSensitivity = Mathf.Max(SettingsSensitivity -= 5f, 15f);
            PopUp.Show("Current sens: " + SettingsSensitivity, 1f);
        }
    }

    private void MouseLook()
    {
        if (Cursor.visible)
            return;

        //float x = Input.GetAxisRaw("Mouse X");
        //float y = Input.GetAxisRaw("Mouse Y");
        Vector2 look = PlayerInputs.Look;

        if (Airship.Crashed)
        {
            playerBody.Rotate(Vector3.up * 10f * Time.deltaTime);
            look.y = 0;
            look.x *= 0.2f;
            //transform.LookAt(Airship.Transform.position);
        }

        playerBody.Rotate(Vector3.up * look.x * sensitivity);
        // Rotates the body horizontally

        if (Airship.Crashed) return;

        yRotation = Mathf.Clamp(yRotation - look.y * sensitivity, -maxVerticalRotation, maxVerticalRotation);
        //float clampedRotWithRecoil = yRotation;
        float clampedRotWithRecoil = Mathf.Clamp(yRotation, -maxVerticalRotation, maxVerticalRotation);

        // Clamps the Y rotation so you can only look straight up or down, not backwards
        transform.localRotation = Quaternion.Euler(new Vector3(clampedRotWithRecoil, 0));
    }

    private void VerticalMovement()
    {
        pos = Vector3.Lerp(pos, Vector3.down * VerticalDip, Time.deltaTime * VertDipSmoothing);
        rot = Quaternion.Slerp(rot, Quaternion.Euler(VerticalDip * VertDipRotationMult, 0, 0), Time.deltaTime * VertDipSmoothing);
        //vertMoveWeaponTransform

        //VerticalDip = Mathf.MoveTowards(VerticalDip, 0, Time.deltaTime * VertDipSpeed);
        VerticalDip = Mathf.Lerp(VerticalDip, 0, Time.deltaTime * VertDipSpeed);
    }

    public static void Nuke()
    {
        instance.Nuke_Instance();
    }

    void Nuke_Instance()
    {
        playerBody.rotation = Quaternion.identity;
        transform.rotation = Quaternion.identity;

        const float CamDist = 450f;
        const float CamHeight = 100f;
        transform.localPosition = new Vector3(0, CamHeight, -CamDist);
        transform.LookAt(Airship.Transform.position);
        playerBody.Rotate(Vector3.up * Random.Range(0, 360));
        //Vector3 pos = Random.insideUnitSphere.Flattened().normalized * CamDist;
        //transform.position = Transform.position + pos + Vector3.up * CamHeight;
        //transform.LookAt(Transform.position);
    }

    /*
    private void SprintShake()
    {
        if (!PlayerMovement.Grounded) return;

        float sin = WeaponSway.SinValue;
        float vert = sin * sin * sin;
        Vector3 desired = new Vector3(Mathf.Abs(vert) * sprintVertShake, -sin * sprintHorShake);
        //Vector3 desired = Vector3.zero;

        sprintRot = Quaternion.Slerp(sprintRot, Quaternion.Euler(desired), Time.deltaTime * 10);
    }
    */
}