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
    [SerializeField] private Texture rawTexture;
    [SerializeField] private Image imgBG;
    
    private float noiseDensity;
    private Material noiseMat;

    private float elapsedTime;

    protected override void Reset()
    {
        base.Reset();
        
        noise = GetComponentInChildren<RawImage>();
        rectTransform = noise.GetComponent<RectTransform>();
        rawTexture = noise.mainTexture;
        imgBG = transform.FindChildByName<Image>("Img_BG");
    }
    
    public override void Initialization()
    {
        base.Initialization();

        // 초기 값
        canvas.planeDistance = 0.8f;
        noise.texture = rawTexture;
        imgBG.enabled = false;
        noise.enabled = true;
        
        noiseMat = noise.material;
        noiseSpeed = -2;
        noiseDuration = 15;

        UIManager.Instance.onUpdateDeathAnimation -= () => Open(3);
        UIManager.Instance.onUpdateDeathAnimation += () => Open(3);
    }

    public void Open(float waitTime)
    {
        base.Open();
        
        StartCoroutine(NoiseAnimation(waitTime));
    }

    public void Close(float endValue)
    {
        canvasGroup.FadeAnimation(endValue, 1f);
    }

    private IEnumerator NoiseAnimation(float waitTime)
    {
        float alpha = 0;
        imgBG.enabled = false;
        noise.enabled = true;
        noise.texture = null;
        noiseMat.color = new Color(0, 0, 0, alpha);
        elapsedTime = 0f;
        noiseDensity = 0f;
        noiseMat.SetFloat(Opacity, noiseDensity);
        
        yield return new WaitForSeconds(waitTime);
        
        base.Open();

        while (elapsedTime < 2f)
        {
            float t = elapsedTime / 3f;
            alpha = Mathf.Clamp01(t);
            noiseMat.color = new Color(0, 0, 0, alpha);

            noiseDensity += Time.unscaledDeltaTime * noiseSpeed;
            noiseMat.SetFloat(Opacity, noiseDensity);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        imgBG.enabled = true;
        noise.enabled = false;
        
        noiseDensity = 0; 
        noiseMat.SetFloat(Opacity, noiseDensity);
    }
    
    public Tween SetScale(float endValue, float duration)
    {
        return rectTransform.DOScale(endValue, duration);
    }
}
