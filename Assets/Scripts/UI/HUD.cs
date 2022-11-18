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
    //public CanvasGroup fuelBar;
    //public Image fuelFill;
    //public Gradient fuelGradient;
    bool interact;
    bool black;
    //bool fuelShow;

    float fuel;


    public static bool ShowHUD = true;

    public static void SetInteract(bool on)
    {
        instance.interact = on;
    }

    public static void SetBlack(bool on)
    {
        instance.black = on;
    }

    /*
    public static void SetFuel(float fuel)
    {
        instance.fuel = fuel;
    }

    public static void SetFuelVisibility(bool on)
    {
        instance.fuelShow = on;
    }
    */

    private void Update()
    {
        interactIcon.alpha = Mathf.Lerp(interactIcon.alpha, interact ? 1 : 0, Time.deltaTime * 10);
        blackScreen.alpha = Mathf.Lerp(blackScreen.alpha, black ? 1 : 0, Time.deltaTime * 10);
        //fuelBar.alpha = Mathf.Lerp(fuelBar.alpha, fuelShow ? 1 : 0, Time.deltaTime * 10);
        //fuelFill.fillAmount = Mathf.Lerp(fuelFill.fillAmount, Remap.Float(fuel, 0, 180, 0, 1), Time.deltaTime * 10f);
        //fuelFill.color = fuelGradient.Evaluate(fuelFill.fillAmount);
        hudHolder.alpha = ShowHUD ? 1f : 0f;
    }
}
