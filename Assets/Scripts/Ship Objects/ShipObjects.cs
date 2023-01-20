using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipObjects : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();
    Vector3 lastVelocity;
    //Dictionary<Transform, Rigidbody> rigidbodies = new Dictionary<Transform, Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (!objects.Contains(other.transform) && other.HasTag("ShipObject"))
        {
            objects.Add(other.transform);
            //rigidbodies.Add(other.transform, other.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objects.Contains(other.transform))
        {
            objects.Remove(other.transform);
            other.GetComponent<Rigidbody>().velocity += lastVelocity;
            //rigidbodies.Remove(other.transform);
        }
    }

    public void MoveObjects(Vector3 delta, float y)
    {
        lastVelocity = delta / Time.deltaTime;

        // Moves kids
        for (int i = objects.Count; i > 0;)
        {
            i--;
            if (objects[i] == null)
                objects.RemoveAt(i);
        }

        foreach (Transform child in objects)
        {
            child.position += delta;
            //rigidbodies[child].MovePosition(child.position + delta);
            //rigidbodies[child].
            child.transform.RotateAround(transform.position, Vector3.up, y * Time.deltaTime);
        }
    }
}
