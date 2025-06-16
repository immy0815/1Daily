using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts;
using UnityEngine;

public class Stage : MonoBehaviour
{
  #region Inspector
  
  [SerializeField, ReadOnly] private int currentWaveIndex = 0; 
  [SerializeField, ReadOnly] private WaveGroup currentWave;
  /// <summary>
  /// 스테이지를 클리어하는 중 걸리는 시간
  /// 단위는 0.1초 단위입니다.
  /// </summary>
  [SerializeField, ReadOnly] protected int takenTime = 0;
  public List<WaveGroup> waves = new();
  [SerializeField] protected Player player;
  
  #endregion
  
  public WaveGroup CurrentWave => currentWave;
  public int TakenTime => takenTime;
  public Player Player => player;
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

    for (var i = 0; i < transform.childCount; i++)
    {
      var child = transform.GetChild(i);
      if (child.gameObject.TryGetComponent<Player>(out var player))
      {
        this.player = player;
      }
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
    player.PlayerCondition.OnDeath += OnPlayerDeath;
  }

  public void StopStage()
  {
    OnStageEnd?.Invoke(StageFinishType.Cancel);
  }
  
  private void OnPlayerDeath()
  {
    player.PlayerCondition.OnDeath -= OnPlayerDeath;
    OnStageEnd?.Invoke(StageFinishType.Failure);
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