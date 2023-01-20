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

            if (note == Note.Type.TheShip)
                Journal.Unlock(Note.Type.Tut_Movement, false);
            else if (note == Note.Type.Alarm)
                Journal.Unlock(Note.Type.Tut_Status, false);
            else if (note == Note.Type.Altitude)
                Journal.Unlock(Note.Type.Tut_Dock, false);
            else if (note == Note.Type.Boiler)
                Journal.Unlock(Note.Type.Tut_Boiler, false);
            else if (note == Note.Type.WhatsThePoint)
                Journal.Unlock(Note.Type.Tut_Grapple, false);
        }

        Destroy(gameObject);
    }

    //Tut_Movement,
    //Tut_Status,
    //Tut_Dock,
    //Tut_Boiler,
    //Tut_Grapple
}
