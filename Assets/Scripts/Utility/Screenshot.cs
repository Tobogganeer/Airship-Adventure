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
        if (Keyboard.current.pKey.wasPressedThisFrame && !Cursor.visible)
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
        yield return new WaitForEndOfFrame();
        string date = DateTime.Now.ToString();
        date = date.Replace("/", "-");
        date = date.Replace(" ", "_");
        date = date.Replace(":", "-");
        string fullPath = "Screenshots/Airship " + date + ".png";
        ScreenCapture.CaptureScreenshot(fullPath, 2);
        yield return null;
        HUD.ShowHUD = true;
    }
}
