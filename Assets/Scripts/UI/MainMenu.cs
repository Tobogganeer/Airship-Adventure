using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Not used rn, gonna be for screenshots
    public GameObject mainHud;

    // VVV Called every time the menu is loaded
    private void Start()
    {
        HUD.SetBlack(false);
        HUD.SetInteract(false);
        //HUD.SetFuelVisibility(false);
        Time.timeScale = 1f; // Time slowed when game is paused, reset it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Set cursor to be visible
    }

    // Should be self explanatory I think VVV
    public void Play()
    {
        SceneManager.LoadLevel(Level.Game);
    }

    public void Quit()
    {
        Application.Quit(0);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
