using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public Transform InteractFrom { get; set; }
    public float mult = 4f;
    public Vector3 axis = Vector3.right;

    bool IInteractable.FixedPosition => true;
    public bool IsInteracting { get; set; }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(axis * (mult * Airship.Turn));
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
    }
}
