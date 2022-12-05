using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantDock : DockingSystem
{
    public bool sellingChuteOpened { get; private set; }
    public int cashToGive { get; private set; }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnGUI()
    {
        // i am so sorry kevin dikis

        GUILayout.BeginVertical();
        if (Docked)
        {
            switch (sellingChuteOpened)
            {
                case true:
                    if (cashToGive > 0)
                    {
                        if (GUILayout.Button("Sell Shit"))
                        {

                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Close Hatch"))
                        {

                        }
                    }
                    break;
                case false:
                    if (GUILayout.Button("Open Hatch"))
                    {
                        
                    }
                    break;
            }
        }
        GUILayout.EndVertical();
    }

    public void AddValueToCart(int value)
    {
        cashToGive += value;
    }
}
