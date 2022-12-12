using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Note")]
public class Note : ScriptableObject
{
    //public Type noteType;

    //[Space]
    //public string shortTitle;
    public string title;
    [TextArea(5, 20)] public string description;

    [Space, TextArea(2, 5)]
    public string internalNotes;

    //public enum Type
    //{
    //    Normal,
    //    Delivery
    //}
}
