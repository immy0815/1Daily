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

    private void Awake()
    {
        noise = GetComponentInChildren<RawImage>();
        noiseMat = noise.material;
    }

    private void Reset()
    {
        noise = GetComponentInChildren<RawImage>();
        noiseMat = noise.material;
    }

    private void OnEnable()
    {
        noiseDensity = 0;
        noiseMat.SetFloat(Opacity, noiseDensity);
    }

    private void Update()
    {
        noiseDensity += Time.unscaledDeltaTime * noiseSpeed;
        noiseMat.SetFloat(Opacity, noiseDensity);
    }

    private void OnDisable()
    {
        noiseDensity = 0;
        noiseMat.SetFloat(Opacity, noiseDensity);
    }
}
