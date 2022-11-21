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
    }

    public CanvasGroup hudHolder;

    [Space]
    public CanvasGroup interactIcon;
    public CanvasGroup blackScreen;
    public CanvasGroup dockingIndicator;
    bool interact;
    bool black;
    bool dock;


    public static bool ShowHUD = true;

    public static void SetInteract(bool on)
    {
        instance.interact = on;
    }

    public static void SetBlack(bool on)
    {
        instance.black = on;
    }

    public static void SetDockIndicator(bool on)
    {
        instance.dock = on;
    }

    private void Update()
    {
        interactIcon.alpha = Mathf.Lerp(interactIcon.alpha, interact ? 1 : 0, Time.deltaTime * 10);
        blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, black ? 1 : 0, Time.deltaTime * 10);
        dockingIndicator.alpha = Mathf.Lerp(dockingIndicator.alpha, dock ? 1 : 0, Time.deltaTime * 10);
        hudHolder.alpha = ShowHUD ? 1f : 0f;
    }
}
