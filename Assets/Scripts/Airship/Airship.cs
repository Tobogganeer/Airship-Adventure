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

    public Rigidbody rb;
    public CharacterController playerController;
    public List<Transform> kiddos;
    public float kidRemoveRange = 40f;
    public Wheel wheel;
    public float moveSpeed = 5f;
    public float turnSpeed = 0.2f;
    public float turnAmount = 20f;

    [Space]
    [Min(0f)]
    public float fuelBurnRate = 1f;
    [Range(0f, 1f)]
    public float startingFuel = 0.5f;
    public float maxFuel = 100f;

    [ReadOnly] public float startingSecondsOfFuel;
    [ReadOnly] public float maxSecondsOfFuel;

    [Space]
    public GrappleHook leftHook;
    public GrappleHook rightHook;
    public float hookGrabbingTurnAmount = 2f;

    [Space]
    [Rename("Pickup Spawn Position")]
    public Transform spawnCrapHere;


    public static float Turn { get; private set; }
    float turnPlusMinus1;

    static float fuel = 50f;
    public static float Fuel
    {
        get => fuel;
        set => fuel = Mathf.Clamp(value, 0, instance.maxFuel);
    }

    bool crashed;

    void Update()
    {
        UpdateFuel();

        float desired = 0f;
        desired += leftHook.GetTurnAmount() * hookGrabbingTurnAmount;
        desired += rightHook.GetTurnAmount() * hookGrabbingTurnAmount;

        if (wheel.IsInteracting)
            desired = PlayerInputs.Movement.x;

        turnPlusMinus1 = Mathf.MoveTowards(turnPlusMinus1, desired, Time.deltaTime * turnSpeed);
        Turn = (EaseInOutQuad(0, 1, (turnPlusMinus1 + 1) / 2f) * 2 - 1) * turnAmount;

        Vector3 delta = (-transform.forward * moveSpeed) * Time.deltaTime;
        MovePlayer(delta, Turn);
        MoveKids(delta, Turn);

        transform.Rotate(Vector3.up * Turn * Time.deltaTime);
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        transform.position += delta;
        //rb.velocity = -transform.forward * moveSpeed;
        //rb.MovePosition(transform.position + delta);

        //MovePlayer(transform.position - pos, turn);
    }

    private void Start()
    {
        IntroSpiel();
        fuel = maxFuel * startingFuel;
    }

    void IntroSpiel()
    {
        Timer.New(0.0f, () => PopUp.Show("Welcome to Airship Game!"));
        Timer.New(4.0f, () => PopUp.Show("Right Click/E/F to interact with the wheel/grapple hooks (+- for sens)", 4.0f));
        Timer.New(8.0f, () => PopUp.Show("Shoot fuel caches with LMB", 3.0f));
        Timer.New(11.0f, () => PopUp.Show("Reach the end sphere, follow the arrows!", 4.0f));
        Timer.New(16.0f, () => PopUp.Show("Good Luck!", 3.0f));
    }

    public static void Crash(string reason, float time)
    {
        if (!instance.crashed)
        {
            Timer.DestroyAll(-1);
            instance.crashed = true;
            PopUp.Show(reason, time);
            HUD.SetBlack(true);
            Timer.Create(time, SceneManager.ReloadCurrentLevel);
        }
    }

    void UpdateFuel()
    {
        Fuel -= Time.deltaTime * fuelBurnRate;
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

    void MoveKids(Vector3 delta, float y)
    {
        for (int i = kiddos.Count; i > 0;)
        {
            i--;
            if (kiddos[i] == null || kiddos[i].position
                .SqrDistance(transform.position) > kidRemoveRange * kidRemoveRange)
                kiddos.RemoveAt(i);
        }

        foreach (Transform child in kiddos)
        {
            child.position += delta;
            child.transform.RotateAround(transform.position, Vector3.up, y * Time.deltaTime);
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, kidRemoveRange);
    }


    private void OnValidate()
    {
        startingSecondsOfFuel = maxFuel * startingFuel / fuelBurnRate;
        maxSecondsOfFuel = maxFuel / fuelBurnRate;
    }
}
