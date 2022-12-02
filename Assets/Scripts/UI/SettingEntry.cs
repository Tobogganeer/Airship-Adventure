using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingEntry : MonoBehaviour
{
    public Slider slider;
    public TMP_Text title;
    public TMP_Text value;

    [Space]
    public float defaultValue;
    public float minValue;
    public float maxValue;
    public bool wholeNumbers;

    public float Value
    {
        get => slider.value;
        set => slider.value = value;
    }

    private void Awake()
    {
        title.text = name;
        slider.maxValue = maxValue;
        slider.minValue = minValue;
        slider.wholeNumbers = wholeNumbers;
        slider.value = defaultValue;
    }

    private void Update()
    {
        value.text = slider.value.ToString("F1"); // Show one decimal place
    }

    public void Default()
    {
        slider.value = defaultValue;
    }
}
