using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Press()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
