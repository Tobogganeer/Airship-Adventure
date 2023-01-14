using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickup : MonoBehaviour, IInteractable
{
    public Note.Type note;

    public bool FixedPosition => false;
    public bool IsInteracting { get; set; }
    public Transform InteractFrom => throw new System.NotImplementedException();
    public void OnInteract()
    {
        PickUp();
    }

    void PickUp()
    {
        if (note != Note.Type.None)
        {
            Journal.Unlock(note);
        }

        Destroy(gameObject);
    }
}
