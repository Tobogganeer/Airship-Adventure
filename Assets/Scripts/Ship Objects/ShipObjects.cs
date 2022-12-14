using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ShipObjects : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (!objects.Contains(other.transform) && other.HasTag("ShipObject"))
        {
            objects.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objects.Contains(other.transform))
            objects.Remove(other.transform);
    }

    public void MoveObjects(Vector3 delta, float y)
    {
        // Moves kids
        foreach (Transform child in objects)
        {
            child.position += delta;
            child.transform.RotateAround(transform.position, Vector3.up, y * Time.deltaTime);
        }
    }
}
