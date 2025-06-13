using System;
using UnityEngine;
using UnityEngine.UI;

public class GUISlider : MonoBehaviour
{
    [SerializeField] private SoundType soundType;
    [SerializeField] private Slider slider;

    private void Reset()
    {
        slider = GetComponent<Slider>();
    }

    private void Awake()
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(ChangeValue);
    }
    
    private void ChangeValue(float value)
    {
        soundType.SetVolume(value * 100);
    }
}
