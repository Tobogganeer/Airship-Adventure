using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static SaveSystem;

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
    public GameObject notePrefab;
    public Transform noteHolder;

    public Dictionary<Note, GameObject> entries = new Dictionary<Note, GameObject>();

    public Note[] notes;

    public Dictionary<Note.Type, Note> noteDict = new Dictionary<Note.Type, Note>();

    bool inited;

    public static void Init()
    {
        Instance.Init_I();
    }

    void Init_I()
    {
        if (inited) return;

        noteDict = new Dictionary<Note.Type, Note>();
        entries = new Dictionary<Note, GameObject>();

        for (int i = 0; i < notes.Length; i++)
        {
            GameObject obj = Instantiate(notePrefab, noteHolder);
            obj.GetComponent<JournalEntry>().note = notes[i];
            entries.Add(notes[i], obj);
            noteDict.Add(notes[i].type, notes[i]);
            obj.SetActive(false);
        }
    }

    private void Awake()
    {
        _instance = this;

        Init_I();
    }

    private void Start()
    {
        _instance = this;
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
        //Note note = JournalCreator.noteDict[noteType];
        //Instance.entries[note].SetActive(true);
        //if (announce)
        //    PopUp.Show($"'{note.title}'", 1f, 1f);

        if (Instance == null)
        {
            Debug.Log("Null instance");
            return;
        }

        if (Instance.noteDict == null)
        {
            Debug.Log("Null note dict");
            return;
        }

        if (Instance.entries == null)
        {
            Debug.Log("Null note entries");
            return;
        }

        if (Instance.noteDict.TryGetValue(noteType, out Note note))
        {
            if (Instance.entries.TryGetValue(note, out GameObject val))
            {
                if (val == null)
                    Debug.LogError("Null obj");
                val.SetActive(true);
            }
            if (announce)
                PopUp.Show(GetTitle(note), 1f, 1f);
        }
    }

    static string GetTitle(Note note)
    {
        Note.Type t = note.type;
        if (t == Note.Type.TheShip || t == Note.Type.Alarm || t == Note.Type.Altitude || t == Note.Type.Boiler || t == Note.Type.WhatsThePoint)
            return $"'{note.title}' (+ tutorial)";
        return $"'{note.title}'";
    }
}
