using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedField : MonoBehaviour
{
    void Start()
    {
        Set();
        Invoke(nameof(Set), 1.0f);
    }

    void Set()
    {
        GetComponent<TMPro.TMP_Text>().text = "Seed: " + ProcGen.instance.main.seed;
    }
}
