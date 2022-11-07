using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : MonoBehaviour
{
    public Vector3 movement;
    public Space space;

    void Update()
    {
        transform.Translate(movement * Time.deltaTime, space);
    }
}
