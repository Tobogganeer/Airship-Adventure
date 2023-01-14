using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    private static Journal _instance;
    public static Journal Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Journal>();
            return _instance;
        }
    }


    public TMPro.TMP_Text title;
    public TMPro.TMP_Text content;

    public Dictionary<Note, GameObject> entries = new Dictionary<Note, GameObject>();

    private void Start()
    {
        //gameObject.SetActive(false);
        gameObject.SetActive(true);
        title.text = string.Empty;
        content.text = string.Empty;

        Unlock(Note.Type.Credits, false);
        Unlock(Note.Type.Tutorial, false);
    }

    public void Press()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


    public void OnEntryPressed(Note note)
    {
        title.text = note.title;
        content.text = note.description;
    }

    public static void Unlock(Note.Type noteType, bool announce = true)
    {
        Note note = JournalCreator.noteDict[noteType];
        Instance.entries[note].SetActive(true);
        if (announce)
            PopUp.Show($"'{note.title}'", 1f, 1f);
    }
}
