using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Airship : MonoBehaviour, ISaveable
{
    // Singleton stuff
    public static Airship instance;
    private void Awake()
    {
        instance = this;
    }

    // ====================
    // Other Values
    // ====================
    [Space(10), Header("==- Values -==")]
    public Vector3 movement = new Vector3(0, 0, 6.5f); // Speed of the ship
    public float turnSpeed = 0.2f; // Turn speed
    public float turnAmount = 20f; // Turn angle
    public float dockingTurnSpeed = 30f;
    public float hookGrabbingTurnAmount = 2f; // When reeling in a cache, the amount the ship should turn towards that direction
    //public float kidRemoveRange = 40f; // Don't drag along objects further away than this
    public float attachedObjectMaxDistance = 50f;

    // ====================
    // Speed Values
    // ====================
    [Space(10), Header("==- Speed -==")]
    public float baseSpeed = 6.5f;
    public float minHeightSpeed = 15f;
    public float maxHeightSpeed = 3.5f;
    public float nitrousSpeedMult = 3f;


    // ====================
    // Fuel Values
    // ====================
    [Space(10), Header("==- Fuel -==")]
    [Min(0f)]
    public float fuelBurnRate = 1f; // How many fuel units are burnt, per second
    public float burnRateHeightMult = 0.5f; // At min height, burn rate is rate * 0.5f, at max, burn rate is rate * 1.5
    [Range(0f, 1f)]
    public float startingFuel = 0.5f; // Percentage (0-1) of how full a tank to start with
    public float maxFuel = 100f; // The max fuel the ship holds
    public float maxNox = 30f;

    [ReadOnly] public float startingSecondsOfFuel; // Inspector values, showing the equivalant
    [ReadOnly] public float maxSecondsOfFuel;       // seconds of fuel, from fuel units
    [ReadOnly] public float maxSecondsOfFuelMinHeight;       // seconds of fuel, from fuel units
    [ReadOnly] public float maxSecondsOfFuelMaxHeight;       // seconds of fuel, from fuel units
    [ReadOnly] public float currentFuelBurnRate;

    // ====================
    // Inspector References
    // ====================
    [Space(10), Header("==- References -==")]
    [Rename("Player Spawn Position")]
    public Transform spawnCrapHere; // Place on the ship to spawn cargo (temporary)
    public Transform enemyPOI;
    public GrappleHook leftHook; // Grapple hooks
    public GrappleHook rightHook;
    public Wheel wheel;
    public CharacterController playerController; // To move the player with the ship
    public ShipObjects shipObjects;
    public List<Transform> attachedObjects; // Objects that the ship should move (crates, rat etc)
    public GameObject nukePrefab;
    public GameObject destroyedShipPrefab;
    //[SerializeField] VisualEffect _SmokeExsaust;
    public VisualEffect[] smokeEffects;
    public ItemPipe leftPipe;
    public ItemPipe rightPipe;


    // ====================
    // Instance Members
    // ====================
    float fuel = 50f;
    float nox = 0f;
    float turnPlusMinus1;
    bool canDock;
    bool docked;
    bool docking;
    bool crashed;

    // ====================
    // Static Values
    // ====================
    public static float Fuel
    {
        get => instance.fuel;
        set => instance.fuel = Mathf.Clamp(value, 0, instance.maxFuel);
    }
    public static float Fuel01
    {
        get => Remap.Float01(Fuel, 0, instance.maxFuel);
        set => Fuel = value * instance.maxFuel;
    }
    public static float Nox
    {
        get => instance.nox;
        set => instance.nox = Mathf.Clamp(value, 0, instance.maxNox);
    }
    public static Transform Transform => instance.transform;
    public static float Turn { get; private set; }
    public static bool CanDock
    {
        get => instance.canDock;
        set => instance.canDock = value;
    }
    public static bool Docked
    {
        get => instance.docked;
        set => instance.docked = value;
    }
    public static bool Docking
    {
        get => instance.docking;
        set => instance.docking = value;
    }
    public static bool Crashed => instance.crashed;

    // ====================
    // 
    // ====================

    static bool showTut = true;

    private void Start()
    {
        if (showTut)
        {
            PopUp.Show("E/F/RMB to pick up notes, view them with ESC", 3, 2f);
            showTut = false;
        }
        //IntroSpiel(); // Tutorial text
        fuel = maxFuel * startingFuel; // Starting fuel
    }

    void Update()
    {
        UpdateFuel();
        Move();
        //UpdateAttachedObjects();
    }


    /*
    void IntroSpiel()
    {
        // Using a timer util class to show the tutorial text
        float offset = 3f;
        Timer.New(0.0f, () => PopUp.Show("Seed: " + ProcGen.instance.main.seed));
        Timer.New(offset + 0.0f, () => PopUp.Show("Welcome to Airship Game!"));
        Timer.New(offset + 4.0f, () => PopUp.Show("Right Click/E/F to interact with the wheel/grapple hooks (+- for sens)", 4.0f));
        Timer.New(offset + 8.0f, () => PopUp.Show("Shoot fuel caches with LMB", 3.0f));
        Timer.New(offset + 11.0f, () => PopUp.Show("Reach the end sphere, follow the arrows!", 4.0f));
        Timer.New(offset + 16.0f, () => PopUp.Show("Good Luck!", 3.0f));
    }
    */

    public static void Crash()//string reason, float time)
    {
        if (!instance.crashed)
        {
            //Timer.DestroyAll(-1);
            instance.crashed = true;
            Music.StopImmediately();
            Timer.Create(1.8f, () => Instantiate(instance.nukePrefab, Transform.position, Quaternion.identity));
            //Timer.Create(3.0f, () => Transform.position -= Vector3.up * 20);
            Timer.Create(2.5f, () => Instantiate(instance.destroyedShipPrefab, Transform.position + Vector3.up * 20, Transform.rotation));
            Timer.Create(3.0f, () => instance.gameObject.SetActive(false));
            AudioManager.Play(new Audio("Nuke Full").SetGlobal());
            FPSCamera.Nuke();
            //PopUp.Show(reason, time);
            //HUD.SetBlack(true);
            //Timer.Create(time, SceneManager.ReloadCurrentLevel);
            // Sets screen to black, shows text, reloads level after time
        }
    }

    void UpdateFuel()
    {
        if (Docked || Docking || crashed) return;

        float height = Altitude.AirshipHeightFactor;
        float rate = Mathf.Lerp(-burnRateHeightMult, burnRateHeightMult, height);

        Fuel = Mathf.Max(Fuel - Time.deltaTime * fuelBurnRate * (1f + rate), 0f);
        currentFuelBurnRate = fuelBurnRate * (1f + rate);

        nox = Mathf.Max(nox - Time.deltaTime, 0f);
        //if (Fuel <= 0)
        //{
        //    Crash("Ran out of fuel! Collect floating caches!", 5f);
        //}
        // Fuel clamps itself
        //HUD.SetFuel(Fuel);
        // Decreases fuel and sets the fuel bar
    }

    //public bool DOCKED;
    //public float TURN;
    public float DELTA;

    void Move()
    {
        //DOCKED = docked;
        //TURN = Turn; Just for testing

        if (crashed) return;

        float desiredTurn = 0f;
        desiredTurn += leftHook.GetTurnAmount() * hookGrabbingTurnAmount;
        desiredTurn += rightHook.GetTurnAmount() * hookGrabbingTurnAmount;
        // Turn the ship towards 

        if (wheel.IsInteracting)
            desiredTurn += PlayerInputs.Movement.x;
        // Turn the ship if the player is interacting with the wheel

        // Easing the turn value so steering is smoothed
        turnPlusMinus1 = Mathf.MoveTowards(turnPlusMinus1, desiredTurn, Time.deltaTime * turnSpeed);
        Turn = (EaseInOutQuad(0, 1, (turnPlusMinus1 + 1) / 2f) * 2 - 1) * turnAmount;

        movement.z = GetSpeed();

        // VVV How much the ship will move
        Vector3 delta = (-transform.forward * movement.z + Vector3.up * movement.y) * Time.deltaTime;

        if (DockingSystem.Docking)
        {
            //delta = Vector3.zero;
            delta = transform.position.DirectionTo_NoNormalize
                (DockingSystem.ActiveSystem.dockTo.position) * Time.deltaTime;
            //desiredTurn = 0;
            turnPlusMinus1 = 0;
            float y = DockingSystem.ActiveSystem.dockTo.eulerAngles.y;
            float deltaAngle = y - transform.eulerAngles.y;
            //Debug.Log("DELTA: " + deltaAngle);
            //if (deltaAngle < -360) deltaAngle += 360;
            //if (deltaAngle > 360) deltaAngle -= 360;
            if (deltaAngle > 180) deltaAngle -= 360;
            if (deltaAngle < -180) deltaAngle += 360;
            DELTA = deltaAngle;
            Turn = Mathf.Clamp(deltaAngle, -dockingTurnSpeed, dockingTurnSpeed);
            //Debug.Log("DELTA CLAMPED: " + Turn);
            //Turn /= 2;

            if (Mathf.Abs(Turn) < 2f)
            {
                Turn = deltaAngle / Time.deltaTime;
                DockingSystem.Docked = true;
                //_SmokeExsaust.Stop();
                foreach (VisualEffect vfx in smokeEffects)
                    vfx.Stop();
                //HUD.SetDepartureIndicator(true);
            }
        }
        else if (DockingSystem.Docked)
        {
            delta = Vector3.zero;
            Turn = 0;
        }
        else if (DockingSystem.RecentlyDocked)
        {
            delta = transform.position.DirectionTo_NoNormalize
                (DockingSystem.ActiveSystem.releaseTo.position) * Time.deltaTime;

            turnPlusMinus1 = 0;
            float y = DockingSystem.ActiveSystem.releaseTo.eulerAngles.y;
            float deltaAngle = y - transform.eulerAngles.y;
            //if (deltaAngle < -360) deltaAngle += 360;
            //if (deltaAngle > 360) deltaAngle -= 360;
            if (deltaAngle > 180) deltaAngle -= 360;
            if (deltaAngle < -180) deltaAngle += 360;
            Turn = Mathf.Clamp(deltaAngle, -dockingTurnSpeed, dockingTurnSpeed);
            //Turn /= 2;

            if (Mathf.Abs(Turn) < 2f)
            {
                DockingSystem.RecentlyDocked = false;
                //HUD.SetDepartureIndicator(true);
            }
        }

        if (DockingSystem.RecentlyDocked == true)
        {
            foreach (VisualEffect vfx in smokeEffects)
                vfx.Play();
        }

        MovePlayer(delta, Turn);
        MoveAttachedObjects(delta, Turn);
        shipObjects.MoveObjects(delta, Turn);
        // ^^^ Move the player and children along with the ship

        // VVV Move and rotate the ship itself
        transform.position += delta;
        transform.Rotate(Vector3.up * Turn * Time.deltaTime);
    }

    void MovePlayer(Vector3 delta, float y)
    {
        playerController.Move(delta);
        playerController.enabled = false;
        //playerController.transform.position += delta;
        playerController.transform.RotateAround(transform.position, Vector3.up, y * Time.deltaTime);
        playerController.enabled = true;
        //playerController.enableOverlapRecovery = false;
        //playerController.
        // Moves and rotates player, must disable cc to rotate player
    }

    void MoveAttachedObjects(Vector3 delta, float y)
    {
        foreach (Transform obj in attachedObjects)
        {
            if (obj.position.SqrDistance(transform.position) >
                attachedObjectMaxDistance * attachedObjectMaxDistance)
            {
                /*
                obj.position = spawnCrapHere.position;
                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                */
                Spawn(obj.gameObject);
            }
        }

        // Moves kids
        foreach (Transform child in attachedObjects)
        {
            child.position += delta;
            child.transform.RotateAround(transform.position, Vector3.up, y * Time.deltaTime);
        }
    }

    public static void MoveAllObjects(Vector3 toPos)//, float toRot)
    {
        Vector3 delta = Transform.position.DirectionTo_NoNormalize(toPos);
        //float y = toRot - Transform.eulerAngles.y;
        float y = 0;

        instance.MovePlayer(delta, y);
        instance.MoveAttachedObjects(delta, y);
        instance.shipObjects.MoveObjects(delta, y);
        // ^^^ Move the player and children along with the ship

        // VVV Move and rotate the ship itself
        Transform.position += delta;
        Transform.Rotate(Vector3.up * y * Time.deltaTime);
    }

    float GetSpeed()
    {
        float height = Altitude.AirshipHeightFactor;
        float noxMult = nox > 0f ? nitrousSpeedMult : 1f;

        if (height < 0.5f)
            return Mathf.Lerp(minHeightSpeed, baseSpeed, height * 2f) * noxMult;
        return Mathf.Lerp(baseSpeed, maxHeightSpeed, (height - 0.5f) * 2f) * noxMult;
        //baseSpeed
    }

    // https://gitlab.com/gamedev-public/unity/-/blob/main/Scripts/Extensions/Easings/EasingUtility.cs
    // Ease function
    public static float EaseInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }

    // Just visual for kid removal range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, kidRemoveRange);
    }


    // Inspector values
    private void OnValidate()
    {
        startingSecondsOfFuel = maxFuel * startingFuel / fuelBurnRate;
        maxSecondsOfFuel = maxFuel / fuelBurnRate;
        maxSecondsOfFuelMinHeight = maxSecondsOfFuel * (1f + burnRateHeightMult);
        maxSecondsOfFuelMaxHeight = maxSecondsOfFuel * (1f - burnRateHeightMult);
    }

    public void Save(ByteBuffer buf)
    {
        buf.Write(transform.position);
        buf.Write(transform.rotation);
        buf.Write(fuel);
        buf.Write(movement);
        buf.Write(turnPlusMinus1);
        buf.Write(Turn);
        // ship objects?
    }

    public void Load(ByteBuffer buf)
    {
        throw new System.NotImplementedException();
    }

    public static void Spawn(GameObject obj)
    {
        if (!obj.TryGetComponent(out Rigidbody rb))
            throw new System.ArgumentException("Spawned object must have a Rigidbody!");

        if (Random.value > 0.5f)
            instance.leftPipe.Spawn(rb);
        else
            instance.rightPipe.Spawn(rb);
    }
}
