using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICrosshair : UIBase
{
    [SerializeField] private RectTransform crosshair;

    protected override void Reset()
    {
        base.Reset();
        
        crosshair = transform.Find("Crosshair")?.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Pistol.OnReloadStart += PlayReloadAnimation;
    }

    private void OnDisable()
    {
        Pistol.OnReloadStart -= PlayReloadAnimation;
    }

    public void PlayReloadAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(RotateCrosshairCoroutine());
    }

    private IEnumerator RotateCrosshairCoroutine()
    {
        Quaternion startRot = crosshair.localRotation;
        Quaternion endRot = Quaternion.Euler(0, 0, -90);
        float duration = 1f;
        float time = 0f;

        while (time < duration)
        {
            crosshair.localRotation = Quaternion.Lerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        crosshair.localRotation = startRot;
    }
}