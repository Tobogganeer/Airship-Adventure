using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void Save()
    {
        ByteBuffer buf = ByteBuffer.Get();
    }

    public class Prefab
    {
        public ID id;
        public GameObject prefab;
    }

    public enum ID
    {
        Player,
        Airship,
        Barrel,
        Crate,
        Lantern,
        NitrousCan,
        Food,
        Water,
        Rat
    }
}
