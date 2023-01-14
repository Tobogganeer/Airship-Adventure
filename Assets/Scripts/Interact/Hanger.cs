using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanger : MonoBehaviour
{
    public float force = 10;

    //const float Offset = 0.25f;
    //Dictionary<GameObject, Rigidbody>

    Dictionary<Transform, Rigidbody> nearby = new Dictionary<Transform, Rigidbody>();
    const string Tag = "Hangable";
    Transform child;

    public int num;
    public int num2;

    private void Start()
    {
        child = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.HasTag(Tag)) return;

        nearby[other.transform] = other.attachedRigidbody;
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearby.ContainsKey(other.transform))
            nearby.Remove(other.transform);
    }

    private void Update()
    {
        num = nearby.Count;
        num2 = 0;
        Transform remove = null;

        foreach (Rigidbody rb in nearby.Values)
        {
            if (Interactor.CurrentInteractable == null
            || (Interactor.CurrentInteractable.transform != null
            && Interactor.CurrentInteractable.transform != rb.transform))
            {
                rb.velocity = rb.transform.position.DirectionTo_NoNormalize(child.position) * force;
                num2++;
            }
            //else
            //    remove = rb.transform;
        }

        if (remove != null)
            nearby.Remove(remove);
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (!other.HasTag(Tag)) return;

        //IInteractable i = Interactor.CurrentInteractable;
        //Debug.Log("I is null: " + (i == null));

        if ((Interactor.CurrentInteractable == null
            || (Interactor.CurrentInteractable.transform != null
            && Interactor.CurrentInteractable.transform != other.transform)))
        {
            //other.attachedRigidbody.AddForce(other.transform.position.DirectionTo_NoNormalize(
            //    child.position) * force, ForceMode.Acceleration);
            other.attachedRigidbody.velocity = other.transform.position.DirectionTo_NoNormalize(child.position) * force;
        }
    }
    */

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position - Vector3.up * Offset, 0.1f);
    //}
}
