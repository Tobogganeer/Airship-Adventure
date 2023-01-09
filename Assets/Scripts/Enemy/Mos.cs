using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mos : MonoBehaviour
{
    public float speed = 12f;
    //public Vector3 rot = new Vector3(0, -90, 0);
    //public Vector3 movement = new Vector3(0, 0, 5f); // Speed of the mos
    //Vector3 desired;
    Quaternion desired;
    bool docked;

    public static int NumEnemys;
    Transform target;

    private void Start()
    {
        target = Airship.instance.enemyPOIs[Random.Range(0, Airship.instance.enemyPOIs.Length)];
    }

    private void LateUpdate()
    {
        if (docked)
        {
            desired = Quaternion.RotateTowards(desired, target.rotation, Time.deltaTime * 180);
            transform.position = target.position;
            //transform.LookAt(target);
            //transform.Rotate(desired);
            transform.rotation = desired;
        }
        else
        {
            transform.LookAt(target);

            transform.position = Vector3.MoveTowards(transform.position, target.position, (Time.deltaTime * speed));
            
            if (Vector3.Distance(transform.position, target.position) < 2)
            {
                docked = true;
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
