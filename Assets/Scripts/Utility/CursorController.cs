using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public bool interactingToggle { get; private set; }

    void Update()
    {
        if (PauseMenu.Paused || DebugConsole.Active || SceneManager.CurrentLevel == Level.MainMenu || interactingToggle)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void IntToggle()
    {
        interactingToggle = !interactingToggle;
    }
}
