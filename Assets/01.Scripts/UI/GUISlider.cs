using UnityEngine;
using UnityEngine.UI;

public class GUISlider : MonoBehaviour
{
    [SerializeField] private SoundType soundType;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(ChangeValue);
    }
    
    private void ChangeValue(float value)
    {
        UIManager.Instance.SetVolume(soundType, value);
    }
}
