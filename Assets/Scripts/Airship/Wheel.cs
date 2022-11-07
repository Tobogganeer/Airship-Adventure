using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public Transform InteractFrom { get; set; }
    public float mult = 4f;

    bool IInteractable.FixedPosition => true;
    public bool IsInteracting { get; set; }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, mult * Airship.Turn);
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
    }
}
