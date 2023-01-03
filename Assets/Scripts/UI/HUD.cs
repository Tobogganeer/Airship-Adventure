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
        //SetBlack(false);
        SetLoading(false);
        SetInteract(false);
        Time.timeScale = 1f; // Time slowed when game is paused, reset it
    }

    public CanvasGroup hudHolder;

    [Space]
    public CanvasGroup interactIcon;
    public CanvasGroup loadingScreen;
    bool interact;
    //bool black;
    bool loading;


    public static bool ShowHUD = true;

    public static void SetInteract(bool on)
    {
        instance.interact = on;
    }

    //public static void SetBlack(bool on)
    //{
    //    instance.black = on;
    //}

    public static void SetLoading(bool on)
    {
        instance.loading = on;
    }

    private void Update()
    {
        interactIcon.alpha = Mathf.Lerp(interactIcon.alpha, interact ? 1 : 0, Time.deltaTime * 10);
        //blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, black ? 1 : 0, Time.deltaTime * 10);
        hudHolder.alpha = ShowHUD ? 1f : 0f;

        if (loading)
            loadingScreen.alpha = 1f;
        else
            loadingScreen.alpha = Mathf.Lerp(loadingScreen.alpha, 0, Time.deltaTime * 5);
    }
}
