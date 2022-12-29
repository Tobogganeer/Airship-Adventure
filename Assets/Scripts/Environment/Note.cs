using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Note")]
public class Note : ScriptableObject
{
    public Type type;

    //[Space]
    //public string shortTitle;
    public string title;
    [TextArea(5, 20)] public string description;

    [Space, TextArea(2, 5)]
    public string internalNotes;

    public enum Type
    {
        None,
        BarrelTossing,
        Boiler,
        Credits,
        Gregory,
        Lights,
        Mountains,
        MrBubbles,
        Music,
        Prometheus,
        Stars,
        SunAndMoon,
        Ted,
        TheBuzz,
        TheShip,
        WhatsThePoint,
        Altitude,
        Alarm,
        Tutorial
    }
}
