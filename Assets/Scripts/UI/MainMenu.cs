using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        HUD.SetBlack(false);
        HUD.SetInteract(false);
        HUD.SetFuelVisibility(false);
    }

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
