using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenFX : MonoBehaviour
{
    private static readonly int Opacity = Shader.PropertyToID("_Opacity");
    [SerializeField] private RawImage noise;
    [SerializeField] float noiseSpeed;
    [SerializeField] private float noiseDuration;

    private float noiseDensity;
    private Material noiseMat;

    private float elapsedTime;
    private void Awake()
    {
        noise = GetComponentInChildren<RawImage>();
        noiseMat = noise.material;
    }

    private void Reset()
    {
        noise = GetComponentInChildren<RawImage>();
        noiseMat = noise.material;
        noiseSpeed = -2;
        noiseDuration = 15;
    }

    private void OnEnable()
    {
        noiseDensity = 0;
        noiseMat.SetFloat(Opacity, noiseDensity);
        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;
        
        if (elapsedTime <= noiseDuration)
        {
            noiseDensity += Time.unscaledDeltaTime * noiseSpeed;

            noiseMat.SetFloat(Opacity, noiseDensity);
        }
    }

    private void OnDisable()
    {
        noiseDensity = 0;
        noiseMat.SetFloat(Opacity, noiseDensity);
    }
}
