using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airship : MonoBehaviour
{
    public static Airship instance;
    private void Awake()
    {
        instance = this;
        HUD.SetBlack(false);
        HUD.SetFuelVisibility(true);
    }

    public CharacterController playerController;
    public Wheel wheel;
    public float moveSpeed = 5f;
    public float speed = 1f;
    public float turnAmount = 15f;

    [Space]
    public float crashDistance = 2f;

    [Space]
    public GrappleHook leftHook;
    public GrappleHook rightHook;
    public float hookGrabbingTurnAmount = 2f;


    public static float Turn { get; private set; }
    float turnPlusMinus1;

    static float fuel = 60f;
    public static float Fuel
    {
        get => fuel;
        set => fuel = Mathf.Clamp(value, 0, MaxFuel);
    }

    const float MaxFuel = 180f;

    bool crashed;

    void Update()
    {
        UpdateFuel();

        float desired = 0f;
        if (leftHook.grabbedTarget != null)
            desired -= hookGrabbingTurnAmount;
        if (rightHook.grabbedTarget != null)
            desired += hookGrabbingTurnAmount;

        if (wheel.IsInteracting)
            desired = PlayerInputs.Movement.x;

        turnPlusMinus1 = Mathf.MoveTowards(turnPlusMinus1, desired, Time.deltaTime * speed);
        Turn = (EaseInOutQuad(0, 1, (turnPlusMinus1 + 1) / 2f) * 2 - 1) * turnAmount;

        Vector3 delta = (-transform.forward * moveSpeed) * Time.deltaTime;
        MovePlayer(delta, Turn);

        transform.Rotate(Vector3.up * Turn * Time.deltaTime);
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        transform.position += delta;
        //rb.MovePosition(transform.position - transform.forward * moveSpeed * Time.deltaTime);

        //MovePlayer(transform.position - pos, turn);

        if (Physics.Raycast(transform.position + Vector3.down * 5f, Vector3.down, out RaycastHit hit))
        {
            if (hit.distance < crashDistance)
            {
                Crash("Crashed into terrain!", 3f);
            }
        }
    }

    private void Start()
    {
        IntroSpiel();
        fuel = 60f;
    }

    void IntroSpiel()
    {
        Timer.New(0.0f, () => PopUp.Show("Welcome to Airship Game!"));
        Timer.New(4.0f, () => PopUp.Show("Right Click/E/F to interact with the wheel/grapple hooks (+- for sens)", 4.0f));
        Timer.New(8.0f, () => PopUp.Show("Shoot fuel caches with LMB", 3.0f));
        Timer.New(11.0f, () => PopUp.Show("Reach the end sphere, follow the arrows!", 4.0f));
        Timer.New(16.0f, () => PopUp.Show("Good Luck!", 3.0f));
    }

    void Crash(string reason, float time)
    {
        if (!crashed)
        {
            Timer.DestroyAll(-1);
            crashed = true;
            PopUp.Show(reason, time);
            HUD.SetBlack(true);
            Timer.Create(time, SceneManager.ReloadCurrentLevel);
        }
    }

    void UpdateFuel()
    {
        Fuel -= Time.deltaTime;
        if (Fuel <= 0)
        {
            Crash("Ran out of fuel! Collect floating caches!", 5f);
        }
        HUD.SetFuel(Fuel);
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
