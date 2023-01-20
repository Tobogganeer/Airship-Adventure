using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour, IInteractable
{
    public Transform handle;
    public float flipSpeed = 10f;
    public float flipAmount = 35f;
    public float flipPause = 0.1f;

    [Space]
    public UnityEvent onInteract;

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

        onInteract?.Invoke();

        flip = true;
    }
}

