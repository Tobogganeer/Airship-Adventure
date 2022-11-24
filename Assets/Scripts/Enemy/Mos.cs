using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mos : MonoBehaviour
{
    public float speed = 6f;

    public Vector3 movement = new Vector3(0, 0, 5f); // Speed of the mos

    public static int NumEnemys;

    float lifeTime;

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        Vector3 delta;

        //delta = transform.position.DirectionTo_NoNormalize
        //        (Airship.instance.enemyPOI.position) * (Time.deltaTime);
        delta = Vector3.MoveTowards(transform.position, Airship.instance.enemyPOI.position, Time.deltaTime * speed);

        transform.position += delta;
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
