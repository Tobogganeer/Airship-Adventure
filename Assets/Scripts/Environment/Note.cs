using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Note")]
public class Note : ScriptableObject
{
    public Type type;

    [Space]
    public string title;
    [TextArea(5, 20)] public string description;

    [Space, TextArea(2, 5)]
    public string internalNotes;

    public enum Type
    {
        Normal,
        Delivery
    }
}
