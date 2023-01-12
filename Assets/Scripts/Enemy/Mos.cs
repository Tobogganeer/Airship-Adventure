using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mos : MonoBehaviour
{
    public float speed = 12f;
    public float FuelsuckRate = 2f;
    public float FuelsuckAmmt = 4f;
    public float minVelocity = 1f;
    public float MosHp = 100f;

    private float Fuelsucktrckr;
    private bool dockedyet;
    private bool DoneorDead;
    bool docked;

    Quaternion desired;


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

        foreach (Transform potentialTarget in Airship.instance.enemyPOIs)
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
            if (transform.position.y < -110)
            {
                Destroy(gameObject);
            }
        }

        if (docked && !DoneorDead)
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
                    Rigidbody rb = GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.AddTorque(Ran(100));
                }

            }

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

    }

    Vector3 Ran(float lim = 360) => new Vector3(
        UnityEngine.Random.Range(-lim, lim),
        UnityEngine.Random.Range(-lim, lim),
        UnityEngine.Random.Range(-lim, lim));

    private void OnCollisionEnter(Collision collision)
    {
        if (DoneorDead) return;
        if (collision.transform.HasTag("NoMosDamage")) return;

        float mass = GetComponent<Rigidbody>().mass;
        float velocity = collision.rigidbody.velocity.magnitude;
        float KE = (mass * 0.5f) * (velocity * velocity);

        if (KE > minVelocity)
        {
            Debug.Log("Hit Item at: " + KE);

            MosHp -= KE;

            if (MosHp < 0)
            {
                Rigidbody rb = GetComponent<Rigidbody>();

                rb.isKinematic = false;
                rb.AddTorque(Ran() * 3);
                rb.velocity = collision.rigidbody.velocity * 2f;

                DoneorDead = true;
            }
        }
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