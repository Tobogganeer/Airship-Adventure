using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    public TMPro.TMP_Text title;
    public TMPro.TMP_Text content;

    private void Start()
    {
        gameObject.SetActive(false);
        title.text = string.Empty;
        content.text = string.Empty;
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
}
