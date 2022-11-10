using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noises : MonoBehaviour
{
    private static Noises _instance;
    public static Noises instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Noises>();

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] private DomainWarp.WarpMode warpMode;
    [SerializeField] private NoiseSettings precipitation;
    [SerializeField] private NoiseSettings temperature;

    public static DomainWarp.WarpMode WarpMode => instance.warpMode;
    public static NoiseSettings Precipitation => instance.precipitation;
    public static NoiseSettings Temperature => instance.temperature;
}
