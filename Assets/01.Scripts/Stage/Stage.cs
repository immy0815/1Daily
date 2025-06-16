using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts;
using Unity.Collections;
using UnityEngine;

public class Stage : MonoBehaviour
{
  [SerializeField, ReadOnly] private int currentWaveIndex = 0; 
  [SerializeField, ReadOnly] private WaveGroup currentWave;
  [SerializeField, ReadOnly] private int takenTime = 0;
  public List<WaveGroup> waves = new();
  public Player player;
  
  public WaveGroup CurrentWave => currentWave;
  public event Action<WaveGroup> OnWaveClear;
  public event Action OnStageClear, OnStageFailure;
  private Coroutine timer;
  
  #region Feature
  
  public void StartStage()
  {
    OnStageClear += () =>
    {
      StopCoroutine(timer);
    };
    timer = StartCoroutine(StartTimer());
    currentWave = waves[currentWaveIndex];
    currentWave.OnClear += StartNextWave;
    currentWave.Spawn();
  }

  private void StartNextWave()
  {
    currentWave.OnClear -= StartNextWave;
    OnWaveClear?.Invoke(currentWave);

    if (currentWaveIndex < waves.Count - 1)
    {
      currentWave = waves[++currentWaveIndex];
      currentWave.OnClear += StartNextWave;
      currentWave.Spawn();
    }
    else if (currentWaveIndex == waves.Count - 1)
    {
      OnStageClear?.Invoke();
    }
  }

  private IEnumerator StartTimer()
  {
    takenTime = 0;
    
    for(;;)
    {
      takenTime += 1;
      yield return new WaitForSeconds(0.1f);
    }
  }
  
  #endregion
}