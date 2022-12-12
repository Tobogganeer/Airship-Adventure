using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanger : MonoBehaviour
{
    public float force = 10;

    //const float Offset = 0.25f;
    //Dictionary<GameObject, Rigidbody>

    const string Tag = "Hangable";
    Transform child;

    private void Start()
    {
        child = transform.GetChild(0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(Tag)) return;

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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position - Vector3.up * Offset, 0.1f);
    //}
}
