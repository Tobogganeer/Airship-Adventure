using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Screenshot : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            StartCoroutine(Capture());
        }
    }

    IEnumerator Capture()
    {
        HUD.ShowHUD = false;
        yield return null;
        //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //string fullPath = Path.Combine(path, "Airship " + DateTime.Now.ToString() + ".png");
        string fullPath = "Airship " + DateTime.Now.ToString() + ".png";
        ScreenCapture.CaptureScreenshot(fullPath, 2);
        yield return null;
        HUD.ShowHUD = true;
    }
}
