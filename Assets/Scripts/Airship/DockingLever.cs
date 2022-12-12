using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingLever : MonoBehaviour, IInteractable
{
    public GameObject indicator;
    public Transform handle;
    public float flipSpeed = 2f;
    public float flipAmount = 45;
    public float flipPause = 1f;
    //public AnimationCurve turnCurve;

    float turn;
    bool flip;
    float pause;
    bool flipReverse;

    public bool FixedPosition => false;
    public Transform InteractFrom => throw new System.NotImplementedException();
    public bool IsInteracting => false;

    private void Start()
    {
        turn = -1f;
    }

    private void Update()
    {
        //float turn0_1 = (turn + 1) / 2f;
        //float curve = turnCurve.Evaluate(turn0_1);
        //float turnN1_1 = curve * 2f - 1f;
        handle.localRotation = Quaternion.Euler(Vector3.right * turn * flipAmount);
        indicator.SetActive(Airship.CanDock || Airship.Docked);

        Flip();
    }

    void Flip()
    {
        if (flip)
        {
            turn += Time.deltaTime * flipSpeed;

            if (turn >= 1)
            {
                turn = 1;
                pause = flipPause;
                flip = false;
                flipReverse = true;
            }
        }
        else if (pause > 0)
        {
            pause -= Time.deltaTime;
        }
        else if (flipReverse)
        {
            turn -= Time.deltaTime * flipSpeed;

            if (turn <= -1)
            {
                turn = -1;
                pause = 0;
                flipReverse = false;
            }
        }
    }

    public void OnInteract()
    {
        if (turn > -1) return;

            //throw new System.NotImplementedException();
        if (Airship.CanDock)
            DockingSystem.Dock();
        else if (Airship.Docked)
            DockingSystem.Release();

        flip = true;
    }
}
