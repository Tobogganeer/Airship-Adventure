using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfObject : MonoBehaviour
{
    [HideInInspector]
    public bool onShelf;

    public float scaleSpeed = 3;
    public float smallSize = 0.25f;
    private Vector3 Normalscale;
    private float currentsize;

    public float ShelfSize = .25f;
    public float NormSize = 1f;

    private void Awake()
    {
        Normalscale = transform.localScale; 
        currentsize = 1;
    }

    private void Update()
    {
        float desired = onShelf ? smallSize : 1.0f;
        currentsize = Mathf.Lerp(currentsize, desired, scaleSpeed * Time.deltaTime);
        transform.localScale = Normalscale * currentsize;
    }

}
