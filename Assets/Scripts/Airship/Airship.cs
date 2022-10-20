using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Airship : MonoBehaviour
{
    public CharacterController playerController;

    public Vector3 movement;
    //public Vector3 rot;

    public float speed = 1f;
    public float turnAmount = 15f;

    public static float Turn { get; private set; }

    void Update()
    {
        float desired = 0f;
        if (Keyboard.current.qKey.isPressed)
            desired -= turnAmount;
        if (Keyboard.current.eKey.isPressed)
            desired += turnAmount;

        Turn = Mathf.Lerp(Turn, desired, Time.deltaTime * speed);

        Vector3 delta = transform.TransformVector(movement) * Time.deltaTime;
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
}
