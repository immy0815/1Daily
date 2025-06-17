using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDeathScreenFX : UIBase
{
    private static readonly int Opacity = Shader.PropertyToID("_Opacity");
    [SerializeField] private RawImage noise;
    [SerializeField] float noiseSpeed;
    [SerializeField] private float noiseDuration;

    private float noiseDensity;
    private Material noiseMat;

    private float elapsedTime;

    protected override void Reset()
    {
        base.Reset();
        
        noise = GetComponentInChildren<RawImage>();
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

    private IEnumerator NoiseAnimation()
    {
        elapsedTime = 0f;
        noiseDensity = 0f;
        noiseMat.SetFloat(Opacity, noiseDensity);
        
        yield return new WaitForSeconds(3f);
        
        base.Open();
        
        while (elapsedTime <= noiseDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            noiseDensity += Time.unscaledDeltaTime * noiseSpeed;

            noiseMat.SetFloat(Opacity, noiseDensity);
            yield return null;
        }
    }
}
