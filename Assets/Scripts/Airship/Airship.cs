using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Airship : MonoBehaviour
{
    public static Airship instance;
    private void Awake()
    {
        instance = this;
    }

    public CharacterController playerController;
    public Wheel wheel;

    public Vector3 movement;
    //public Vector3 rot;

    public float speed = 1f;
    public float turnAmount = 15f;

    public static float Turn { get; private set; }
    float turnPlusMinus1;

    void Update()
    {
        float desired = 0f;
        if (wheel.IsInteracting)
            desired = PlayerInputs.Movement.x;

        turnPlusMinus1 = Mathf.MoveTowards(turnPlusMinus1, desired, Time.deltaTime * speed);
        Turn = (EaseInOutQuad(0, 1, (turnPlusMinus1 + 1) / 2f) * 2 - 1) * turnAmount;

        Vector3 delta = (transform.TransformVector(movement) / transform.localScale.z) * Time.deltaTime;
        MovePlayer(delta, Turn);

        transform.Rotate(Vector3.up * Turn * Time.deltaTime);
        transform.Translate(movement * Time.deltaTime, Space.Self);

        //MovePlayer(transform.position - pos, turn);
    }

    void MovePlayer(Vector3 delta, float y)
    {
        playerController.Move(delta);
        playerController.enabled = false;
        playerController.transform.RotateAround(transform.position, Vector3.up, y * Time.deltaTime);
        playerController.enabled = true;
    }

    // https://gitlab.com/gamedev-public/unity/-/blob/main/Scripts/Extensions/Easings/EasingUtility.cs
    public static float EaseInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }


}
