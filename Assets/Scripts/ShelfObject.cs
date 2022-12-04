using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShelfObject : MonoBehaviour
{
    [HideInInspector]
    public float Desiredsize;

    public float scaleSpeed = 3;
    private Vector3 Normalscale;
    private float currentsize;

    private void Awake()
    {
        Desiredsize = 1;
        Normalscale = transform.localScale; 
        currentsize = 1;
    }

    private void Update()
    {
        currentsize = Mathf.Lerp(currentsize, Desiredsize, scaleSpeed * Time.deltaTime);
        transform.localScale = Normalscale * currentsize;
    }

}
