using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altitude : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public Transform InteractFrom { get; set; }
    public float speed = 0.1f;

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

    float height = 0.5f;

    //public static float AirshipHeightFactor { get; private set; }
    public static float AirshipHeightFactor { get; private set; }

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
        if (IsInteracting)
        {
            height += PlayerInputs.Movement.y * speed * Time.deltaTime;
            height = Mathf.Clamp01(height);
        }

        heightGauge.localPosition = heightGauge.localPosition.WithY(Remap.Float(height, 0, 1, -gaugeRange, gaugeRange));
        AirshipHeightFactor = Mathf.InverseLerp(-actualHeightRange, actualHeightRange, Airship.Transform.position.y);
        AirshipHeightFactor = Mathf.Clamp01(AirshipHeightFactor);
        shipGauge.localPosition = shipGauge.localPosition.WithY(Remap.Float(AirshipHeightFactor, 0, 1, -gaugeRange, gaugeRange));

        mat.SetFloat("_Base_Height", Remap.Float(height, 0, 1, -materialHeightRange, materialHeightRange) + materialBase);

        UpdateAirshipSpeed();
    }

    void UpdateAirshipSpeed()
    {
        float desired = Remap.Float(height, 0, 1, -actualHeightRange, actualHeightRange);
        float delta = desired - Airship.Transform.position.y;
        Airship.instance.movement.y = Mathf.Lerp(Airship.instance.movement.y, Mathf.Clamp(delta, -maxAirshipSpeed, maxAirshipSpeed), Time.deltaTime * airshipAccel);

        if (Airship.Docked)
            Airship.instance.movement.y = 0;
    }

    public void OnInteract()
    {
        IsInteracting = !IsInteracting;
    }
}
