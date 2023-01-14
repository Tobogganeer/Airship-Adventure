using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalEntry : MonoBehaviour
{
    public Note note;
    public TMPro.TMP_Text title;

    //Journal journal;

    private void Start()
    {
        //journal = FindObjectOfType<Journal>(); // SCREW YOUR SINGLETON
        // I take that back :P
        title.text = note.name;
    }

    public void Press()
    {
        Journal.Instance.OnEntryPressed(note);
        //journal.OnEntryPressed(note);
    }
}
