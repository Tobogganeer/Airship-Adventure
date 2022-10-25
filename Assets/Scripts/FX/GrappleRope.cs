using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleRope : MonoBehaviour
{
    private Spring spring;
    public Vector3 start;
    public Vector3 end;
    public LineRenderer lr;
    public int quality = 3;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve effectCurve;

    [HideInInspector]
    public bool show;

    private void Start()
    {
        spring = new Spring();
        spring.SetTarget(0);
    }

    /*
    void LateUpdate()
    {
        DrawRope();
    }
    */

    public void DrawRope()
    {
        /*
        //If not grappling, don't draw rope
        if (!gadgetLauncher.IsGrappling())
        {
            currentGrapplePosition = gadgetLauncher.grappleTip.position;
            spring.Reset();
            if (grappleLineRenderer.positionCount > 0)
                grappleLineRenderer.positionCount = 0;
            return;
        }
        */

        if (!show)
        {
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 up = Quaternion.LookRotation((end - start).normalized) * Vector3.up;

        //currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, end, Time.deltaTime * 12f);

        for (int i = 0; i < quality + 1; i++)
        {
            float delta = i / (float)quality;
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * effectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(start, end, delta) + offset);
        }
    }
}
