using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleCam : MonoBehaviour
{
    public GrappleHook hook;
    public float moveSpeed = 5f;

    [HideInInspector]
    public bool aimingAtTarget;

    [Space]
    public GameObject greenObj;
    public GameObject redObj;


    FPSCamera cam;
    Transform camParent;
    float fac;
    bool atHome;

    const float Epsilon = 0.05f;

    private void Start()
    {
        cam = FPSCamera.instance;
        camParent = cam.transform.parent;
    }

    void LateUpdate()
    {
        if (Airship.Crashed) return;

        if (hook.IsInteracting)
            //fac += Time.deltaTime * moveSpeed;
            fac = Mathf.Lerp(fac, 1, Time.deltaTime * moveSpeed);
        else
            //fac -= Time.deltaTime * moveSpeed;
            fac = Mathf.Lerp(fac, 0, Time.deltaTime * moveSpeed);

        //fac = Mathf.Clamp01(fac);

        //Vector3 thisPosInCamLocalSpace = cam.transform.InverseTransformPoint(transform.position);
        //Vector3 zeroInWorldSpace = cam.transform.TransformPoint(Vector3.zero);
        if (fac > Epsilon)
        {
            //cam.transform.localPosition = Vector3.Lerp(Vector3.zero, thisPosInCamLocalSpace, fac);
            cam.transform.position = Vector3.Lerp(camParent.position, transform.position, fac);
            atHome = false;
        }
        else if (!atHome)
        {
            atHome = true;
            cam.transform.localPosition = Vector3.zero;
        }

        greenObj.SetActive(aimingAtTarget);
        redObj.SetActive(!aimingAtTarget);
    }
}
