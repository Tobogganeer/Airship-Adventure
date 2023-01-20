using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altitude : MonoBehaviour, IInteractable
{
    public static Altitude Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [field: SerializeField]
    public Transform InteractFrom { get; set; }
    public float speed = 0.1f;
    public float noFuelSpeedMult = 0.3f;
    public float tiltSpeed = 0.2f;
    public float maxTilt = 20f;

    [Space]
    public Transform heightGauge;
    public Transform shipGauge;

    public float gaugeRange;
    //public float baseAirshipHeight = 70; // Sea level @ -70
    public float actualHeightRange = 50;
    public float materialHeightRange;
    public float materialBase = -0.19f;
    public Renderer mapPanel;
    Material mat;

    [Space]
    public float maxAirshipSpeed = 3f;
    public float airshipAccel = 0.3f;

    public float height = 0.5f;
    float fuelHeight = 0.5f;
    float tilt;

    //public static float AirshipHeightFactor { get; private set; }
    public static float AirshipHeightFactor { get; private set; }
    public static float DesiredAirshipHeight { get; private set; }

    bool IInteractable.FixedPosition => true;
    public bool IsInteracting { get; set; }

    private void Start()
    {
        mat = mapPanel.material;
    }

    private void OnDestroy()
    {
        Destroy(mat);
    }

    void Update()
    {
        //if (Airship.Fuel <= 0)
        FPSCamera.SetFOV(IsInteracting ? 0.4f : 1f);

        if (Airship.Fuel <= 0 && !Airship.Docked)
        {
            IsInteracting = false;
            //height -= speed * noFuelSpeedMult * Time.deltaTime;
            fuelHeight -= speed * noFuelSpeedMult * Time.deltaTime;
            fuelHeight = Mathf.Clamp(fuelHeight, -0.25f, height); // Clamp lower to enable crashing
            //height = Mathf.Clamp(height, -0.5f, 1f); // Clamp lower to enable crashing
            //tilt += tiltSpeed * Time.deltaTime;
            //tilt = Mathf.Clamp(tilt, 0, 15);
            tilt = Mathf.Lerp(tilt, maxTilt, Time.deltaTime * tiltSpeed);
        }
        else
        {
            fuelHeight += speed * noFuelSpeedMult * Time.deltaTime;
            fuelHeight = Mathf.Clamp(fuelHeight, -0.5f, height);
            tilt = Mathf.Lerp(tilt, 0, Time.deltaTime * tiltSpeed);
        }
        
        if (IsInteracting)
        {
            height += PlayerInputs.Movement.y * speed * Time.deltaTime;
            height = Mathf.Clamp01(height);
            //tilt -= tiltSpeed * Time.deltaTime;
            //tilt = Mathf.Clamp(tilt, 0, 15);
        }

        float usedHeight = Airship.Fuel <= 0 ? fuelHeight : height;

        Airship.Transform.localRotation = Quaternion.Euler(Airship.Transform.localEulerAngles.WithX(-tilt));
        heightGauge.localPosition = heightGauge.localPosition.WithY(Remap.Float(usedHeight, 0, 1, -gaugeRange, gaugeRange));
        AirshipHeightFactor = Mathf.InverseLerp(-actualHeightRange, actualHeightRange, Airship.Transform.position.y);
        AirshipHeightFactor = Mathf.Clamp01(AirshipHeightFactor);
        shipGauge.localPosition = shipGauge.localPosition.WithY(Remap.Float(AirshipHeightFactor, 0, 1, -gaugeRange, gaugeRange));

        mat.SetFloat("_Base_Height", Remap.Float(height, 0, 1, -materialHeightRange, materialHeightRange) + materialBase);

        UpdateAirshipSpeed();
    }

    void UpdateAirshipSpeed()
    {
        float usedHeight = Airship.Fuel <= 0 ? fuelHeight : height;
        float desired = Remap.Float(usedHeight, 0, 1, -actualHeightRange, actualHeightRange);
        DesiredAirshipHeight = desired;
        float delta = desired - Airship.Transform.position.y;
        float extraAccel = 1f;
        if (delta < 10f)
        {
            extraAccel = 3f;
        }
        Airship.instance.movement.y = Mathf.Lerp(Airship.instance.movement.y, Mathf.Clamp(delta, -maxAirshipSpeed, maxAirshipSpeed), Time.deltaTime * airshipAccel * extraAccel);

        if (Airship.Docked)
            Airship.instance.movement.y = 0;
    }

    public void OnInteract()
    {
        if (Airship.Fuel <= 0)
            return;
        IsInteracting = !IsInteracting;
    }
}
