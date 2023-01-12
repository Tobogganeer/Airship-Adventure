using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{
    public Color nearColour = Color.green;
    public Color farColour = Color.red;

    public float nearbyRange = 5f;
    public Transform compHeightTo;

    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void OnDestroy()
    {
        Destroy(mat);
    }

    private void Update()
    {
        if (Mathf.Abs(Altitude.DesiredAirshipHeight - compHeightTo.position.y) < nearbyRange)
        {
            mat.color = nearColour;
        }
        else
        {
            mat.color = farColour;
        }
    }
}
