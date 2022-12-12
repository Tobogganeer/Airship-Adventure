using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseObj;
    public float timeSlow = 0.1f;
    public static bool Paused;

    private void OnEnable()
    {
        SetPaused(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetPaused(!Paused);
        }
    }

    private void OnDisable()
    {
        SetPaused(false);
    }

    public void Resume()
    {
        SetPaused(false);
    }

    public void Quit()
    {
        SetPaused(false);
        SceneManager.LoadLevel(Level.MainMenu);
    }

    public void SetPaused(bool paused)
    {
        Paused = paused;
        Time.timeScale = Paused ? timeSlow : 1f;
        pauseObj.SetActive(Paused);
        //Cursor.visible = paused;
        //Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
