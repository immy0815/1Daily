using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIDeathScreenFX : UIBase
{
    private static readonly int Opacity = Shader.PropertyToID("_Opacity");
    [SerializeField] private RawImage noise;
    [SerializeField] float noiseSpeed;
    [SerializeField] private float noiseDuration;
    [SerializeField] RectTransform rectTransform;
    
    private float noiseDensity;
    private Material noiseMat;

    private float elapsedTime;

    protected override void Reset()
    {
        base.Reset();
        
        noise = GetComponentInChildren<RawImage>();
        rectTransform = noise.GetComponent<RectTransform>();
    }
    
    public override void Initialization()
    {
        base.Initialization();
        
        noiseMat = noise.material;
        noiseSpeed = -2;
        noiseDuration = 15;

        UIManager.Instance.onUpdateDeathAnimation -= Open;
        UIManager.Instance.onUpdateDeathAnimation += Open;
    }

    public override void Open()
    {
        StartCoroutine(NoiseAnimation());
    }
    
    public override void Close()
    {
        base.Close();
        
        noiseDensity = 0; 
        noiseMat.SetFloat(Opacity, noiseDensity);
    }

    public void Close(float endValue)
    {
        canvasGroup.FadeAnimation(endValue);
        
        Close();
    }

    private IEnumerator NoiseAnimation()
    {
        float alpha = 0;
        noiseMat.color = new Color(0, 0, 0, alpha);
        elapsedTime = 0f;
        noiseDensity = 0f;
        noiseMat.SetFloat(Opacity, noiseDensity);
        
        yield return new WaitForSeconds(3f);
        
        base.Open();

        while (elapsedTime < 3f)
        {
            float t = elapsedTime / 3f;
            alpha = Mathf.Clamp01(t);
            noiseMat.color = new Color(0, 0, 0, alpha);

            noiseDensity += Time.unscaledDeltaTime * noiseSpeed;
            noiseMat.SetFloat(Opacity, noiseDensity);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    
    public Tween SetScale(float endValue, float duration)
    {
        return rectTransform.DOScale(endValue, duration);
    }
}
