using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts;
using UnityEngine;
using UnityEngine.Events;

[ AddComponentMenu( "Stage/Stage" )]
public class Stage : MonoBehaviour
{
  private static GameObject ClearOrb => StageManager.ClearOrb;

  #region Inspector
  
  [SerializeField, ReadOnly, Tooltip("현재 메인 카메라입니다.")] private new Camera camera;
  [SerializeField, ReadOnly, Tooltip("현재 웨이브의 순서입니다.")] private int currentWaveIndex = 0; 
  [SerializeField, ReadOnly, Tooltip("현재 진행중인 웨이브입니다. Reset시 자동으로 설정됩니다.")] private WaveGroup currentWave;
  /// <summary>
  /// 스테이지를 클리어하는 중 걸리는 시간
  /// 단위는 0.1초 단위입니다.
  /// </summary>
  [SerializeField, ReadOnly, Tooltip("현재 스테이지의 진행시간입니다.")] protected int takenTime = 0;
  [Tooltip("스테이지의 적 웨이브 정보입니다. Reset시 자동으로 설정됩니다.")] public List<WaveGroup> waves = new();
  [SerializeField, Tooltip("스테이지 시작시 배치되는 플레이어입니다. Reset시 자동으로 설정됩니다.")] protected Player player;
  
  #endregion
  
  /// <summary>
  /// 현재 진행중인 웨이브입니다.
  /// </summary>
  public WaveGroup CurrentWave => currentWave;
  
  /// <summary>
  /// 스테이지의 진행시간입니다.
  /// </summary>
  public int TakenTime => takenTime;
  
  /// <summary>
  /// 스테이지의 플레이어입니다.
  /// </summary>
  public Player Player => player;
  
  /// <summary>
  /// 적 웨이브가 클리어됬을 때 호출되는 이벤트입니다.
  /// </summary>
  public UnityEvent<WaveGroup> OnWaveClear = new();
  
  /// <summary>
  /// 스테이지가 끝났을 때 호출되는 이벤트입니다.
  /// </summary>
  public UnityEvent<StageFinishState> OnStageEnd = new();
  private Coroutine timer = null;
  
  #region Unity Event
  
  #if UNITY_EDITOR

  private void Reset()
  {
    Transform waveContainer = null;

    for (var i = 0; i < transform.childCount; i++)
    {
      var child = transform.GetChild(i);
      if (child.gameObject.name == "Wave")
      {
        waveContainer = child;
        continue;
      }
      
      if (child.gameObject.TryGetComponent<Player>(out var player))
      {
        this.player = player;
      }
    }
    
    if (waveContainer)
    {
      waves.Clear();
      
      for (var i = 0; i < waveContainer.childCount; i++)
      {
        var child = waveContainer.GetChild(i);
        if (child.gameObject.TryGetComponent<WaveGroup>(out var waveGroup))
        {
          waves.Add(waveGroup);
          waveGroup.Reset();
        }
      }

      if (waves.Count > 0)
        currentWave = waves[0];
    }
  }
  
  #endif

  private void Awake()
  {
    camera = Camera.main;
    
    OnStageEnd.AddListener(state =>
    {
      if (state == StageFinishState.Clear)
      {
        var spawnPos = player.transform.position + camera.transform.forward * 3;
        // 소환 위치 계산: 카메라 위치 + (전방 방향 벡터 * 거리)
        var orb = Instantiate(ClearOrb, spawnPos.Y(camera.transform.position.y), Quaternion.identity);
        orb.transform.parent = transform;
        orb.SetActive(true);
      }
    });
  }
  
  #endregion

  #region Feature
  
  /// <summary>
  /// 스테이지가 진행중이지 않을 경우 시작시킵니다.
  /// </summary>
  public void StartStage()
  {
    if (timer != null) return;
    OnStageEnd.AddListener((_) =>
    {
      if (timer != null)
      {
        StopCoroutine(timer);
      }
      timer = null;
    });
    timer = StartCoroutine(StartTimer());
    currentWave = waves[currentWaveIndex];
    currentWave.OnClear += StartNextWave;
    currentWave.Spawn();
    player.PlayerCondition.OnDeath += OnPlayerDeath;
  }

  /// <summary>
  /// 스테이지를 강제로 중지시킬 수 있습니다.
  /// OnStageEnd 이벤트를 state 매개변수로 호출합니다.
  /// </summary>
  public void StopStage(StageFinishState state = StageFinishState.Cancel)
  {
    OnStageEnd?.Invoke(state);
  }

  #region Use for binding
  [ContextMenu("Clear Stage")]
  public void StageClear() => StopStage(StageFinishState.Clear);
  public void StageFailure() => StopStage(StageFinishState.Failure);
  public void StageCancel() => StopStage(StageFinishState.Cancel);
  
  #endregion

  public void StartStage(int stageIndex)
  {
    StageManager.StartStageStatic(stageIndex);
  }
  
  private void OnPlayerDeath()
  {
    player.PlayerCondition.OnDeath -= OnPlayerDeath;
    OnStageEnd?.Invoke(StageFinishState.Failure);
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
      OnStageEnd?.Invoke(StageFinishState.Clear);
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

/// <summary>
/// 스테이지가 끝났을 때 호출하는 이벤트의 인자들입니다.
/// </summary>
public enum StageFinishState
{
  // 스테이지를 클리어했을 때 넘겨줍니다.
  Clear = 1,
  // 스테이지를 실패했을 때 넘겨줍니다.
  Failure = 2,
  // 스테이지를 취소했을 때 넘겨줍니다.
  Cancel = 3
}