using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mos : MonoBehaviour
{
    public float speed = 12f;
    public float FuelsuckRate = 2f;
    public float FuelsuckAmmt = 4f;
    private float Fuelsucktrckr;
    private bool dockedyet;
    private bool DoneorDead;
    //public Vector3 rot = new Vector3(0, -90, 0);
    //public Vector3 movement = new Vector3(0, 0, 5f); // Speed of the mos
    //Vector3 desired;
    Quaternion desired;
    bool docked;

    public static int NumEnemys;
    Transform target;

    private void Start()
    {
        Fuelsucktrckr = 0;
        dockedyet = false;
        DoneorDead = false;

        //target = Airship.instance.enemyPOIs[Random.Range(0, Airship.instance.enemyPOIs.Length)];

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(Transform potentialTarget in Airship.instance.enemyPOIs)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                target = potentialTarget;
            }
        }


    }

    private void LateUpdate()
    {
        if (DoneorDead)
        {
            transform.position -= new Vector3(0, 15, 0) * Time.deltaTime;
            if ( transform.position.y < -110)
            {
                Destroy(gameObject);
            }
        }

        if (docked)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, Time.deltaTime * 45);
            transform.position = target.position;

            if (transform.rotation == target.rotation)
            {
                Airship.Fuel -= FuelsuckRate * Time.deltaTime;
                Fuelsucktrckr += FuelsuckRate * Time.deltaTime;
                if (FuelsuckAmmt < Fuelsucktrckr)
                {
                    docked = false;
                    DoneorDead = true;
                    FuelsuckRate = 0f;
                }

            }


            // When destoryed
            //transform.position -= new Vector3(0, 15, 0) * Time.deltaTime;


            /* private void OnCollisionEnter(Collision collision)
            {
                float mag = collision.relativeVelocity.magnitude;
                if (mag < minVelocity) return;
            }
            */

            //Check ridged body mass and make that affect that with the speed, this is to prevent the latern from killing the mos 

        }
        else
        {

            if (!DoneorDead)
            {

                transform.LookAt(target);

                transform.position = Vector3.MoveTowards(transform.position, target.position, (Time.deltaTime * speed));

            }

            if (Vector3.Distance(transform.position, target.position) < .1f || dockedyet == false)
            {
                docked = true;
                dockedyet = true;
            }
        }

        //delta = transform.position.DirectionTo_NoNormalize
        //        (Airship.instance.enemyPOI.position) * (Time.deltaTime);
        
    }

    private void OnEnable()
    {
        NumEnemys++;
    }

    private void OnDisable()
    {
        NumEnemys--;
    }
}
