using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
  [SerializeField, ReadOnly] private int currentWaveIndex = 0; 
  [SerializeField, ReadOnly] private WaveGroup currentWave;
  /// <summary>
  /// 스테이지를 클리어하는 중 걸리는 시간
  /// 단위는 0.1초 단위입니다.
  /// </summary>
  [SerializeField, ReadOnly] private int takenTime = 0;
  public List<WaveGroup> waves = new();
  
  public WaveGroup CurrentWave => currentWave;
  public int TakenTime => takenTime;
  public event Action<WaveGroup> OnWaveClear;
  public event Action<StageFinishType> OnStageEnd;
  private Coroutine timer;
  
  #region Unity Event
  
  #if UNITY_EDITOR

  private void Reset()
  {
    var waveContainer = transform.GetChild(2);

    if (waveContainer)
    {
      waves.Clear();
      
      for (var i = 0; i < waveContainer.childCount; i++)
      {
        var child = waveContainer.GetChild(i);
        if (child.gameObject.TryGetComponent<WaveGroup>(out var waveGroup))
        {
          waves.Add(waveGroup);
        }
      }

      if (waves.Count > 0)
        currentWave = waves[0];
    }
  }
  
  #endif
  
  #endregion
  
  
  #region Feature
  
  public void StartStage()
  {
    OnStageEnd += (_) =>
    {
      StopCoroutine(timer);
    };
    timer = StartCoroutine(StartTimer());
    currentWave = waves[currentWaveIndex];
    currentWave.OnClear += StartNextWave;
    currentWave.Spawn();
  }

  public void StopStage()
  {
    OnStageEnd?.Invoke(StageFinishType.Cancel);
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
      OnStageEnd?.Invoke(StageFinishType.Clear);
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

public enum StageFinishType
{
  Clear,
  Failure,
  Cancel
}