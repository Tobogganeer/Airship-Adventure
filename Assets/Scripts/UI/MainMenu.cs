using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Not used rn, gonna be for screenshots
    //public GameObject mainHud;
    public TMPro.TMP_InputField seedField;

    // VVV Called every time the menu is loaded
    private void Start()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        // Set cursor to be visible
    }

    // Should be self explanatory I think VVV
    public void Play()
    {
        if (seedField.text.Length > 0)
            ProcGen.mainMenuSeed = int.Parse(seedField.text);
        else
            ProcGen.mainMenuSeed = 0;
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
