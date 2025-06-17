using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LensDistortionController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private LensDistortion lensDistortion;

    private void Reset()
    {
        volume = GetComponent<Volume>();
    }
    
    private void Start()
    {
        if (volume.profile.TryGet(out lensDistortion))
        {
            lensDistortion.intensity.overrideState = true; // 값을 덮어쓸 수 있게 함
            lensDistortion.intensity.value = 0f; // 초기값
        }
        else
        {
            Debug.LogWarning("Lens Distortion is not exist");
        }
    }

    // Lens Distortion의 Intensity 값을 from ->  to로 duration동안 변화해주는 메서드 
    public Tween DOIntensity(float duration, float from = 0.5f, float to = 0f)
    {
        float current = from;

        return DOTween.To(() => current,
            x =>
            {
                current = x;
                if (lensDistortion != null)
                {
                    lensDistortion.intensity.value = x;
                }
            },
            to, duration)
            .SetUpdate(true);
    }
}
