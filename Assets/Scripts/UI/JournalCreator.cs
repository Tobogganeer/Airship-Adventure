using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalCreator : MonoBehaviour
{
    public GameObject prefab;
    //public Note[] notes;

    //public static Dictionary<Note.Type, Note> noteDict = new Dictionary<Note.Type, Note>();

    void Start()
    {
        /*
        noteDict.Clear();

        for (int i = 0; i < notes.Length; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.GetComponent<JournalEntry>().note = notes[i];
            Journal.Instance.entries.Add(notes[i], obj);
            noteDict.Add(notes[i].type, notes[i]);
            obj.SetActive(false);
        }
        */
    }
}
