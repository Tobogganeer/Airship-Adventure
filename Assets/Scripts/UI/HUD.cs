using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private static HUD instance;
    private void Awake()
    {
        instance = this;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        SetBlack(false);
        SetInteract(false);
        Time.timeScale = 1f; // Time slowed when game is paused, reset it
    }

    public CanvasGroup hudHolder;

    [Space]
    public CanvasGroup interactIcon;
    public CanvasGroup blackScreen;
    bool interact;
    bool black;


    public static bool ShowHUD = true;

    public static void SetInteract(bool on)
    {
        instance.interact = on;
    }

    public static void SetBlack(bool on)
    {
        instance.black = on;
    }

    private void Update()
    {
        interactIcon.alpha = Mathf.Lerp(interactIcon.alpha, interact ? 1 : 0, Time.deltaTime * 10);
        blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, black ? 1 : 0, Time.deltaTime * 10);
        hudHolder.alpha = ShowHUD ? 1f : 0f;
    }
}
