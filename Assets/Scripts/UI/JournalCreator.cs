using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalCreator : MonoBehaviour
{
    public GameObject prefab;
    public Note[] notes;

    void Start()
    {
        for (int i = 0; i < notes.Length; i++)
        {
            Instantiate(prefab, transform).GetComponent<JournalEntry>().note = notes[i];
        }
    }
}
