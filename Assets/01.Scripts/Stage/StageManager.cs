using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class StageManager
{
  private static Stage currentStage = null;
  
  /// <summary>
  /// 현재 진행중인 스테이지입니다.
  /// 스테이지가 진행중이 아닐경우 null을 반환합니다.
  /// </summary>
  public static Stage CurrentStage => currentStage;
  /// <summary>
  /// 스테이지 시작시 이벤트입니다.
  /// 인자로 시작하려는 스테이지를 넘겨줍니다.
  /// </summary>
  public static event Action<Stage> OnStageStart;
  
  /// <summary>
  /// 스테이지 종료 시 이벤트입니다.
  /// 해당 이벤트 호출 시점에는 currentStage가 null이 아닙닌다.
  /// </summary>
  public static event Action<StageFinishState> OnStageEnd;
  
  /// <summary>
  /// 스테이지를 시작할 수 있습니다.
  /// 게임씬을 로딩 후 시작시킵니다.
  /// </summary>
  /// <param name="stageIndex"></param>
  /// <returns></returns>
  public static Stage StartStage(int stageIndex)
  {
    var sceneName = SceneManager.GetActiveScene().name;
    
    if (sceneName == "GameScene") StopStage();
    
    SceneManager.LoadScene("GameScene");
    
    if (ResourceManager.Instance.InstantiateStage($"Stage{stageIndex}", out var obj))
    {
      var stage = obj.GetComponent<Stage>();
      obj.transform.position = Vector3.zero;
      currentStage = stage;
      stage.StartStage();
      OnStageStart?.Invoke(stage);
      stage.OnStageEnd += OnStageFinish;
    }
    else
    {
      SceneManager.LoadScene(sceneName);
#if UNITY_EDITOR
      Debug.LogError("Stage not found");
#endif
    }
    
    return currentStage;
  }

  /// <summary>
  /// 현재 진행중인 스테이지가 있다면 강제로 종료시킵니다.
  /// </summary>
  public static void StopStage()
  {
    if(currentStage)
    {
      currentStage.StopStage();
      Object.Destroy(currentStage.gameObject);
      currentStage = null;
    }
  }

  private static void OnStageFinish(StageFinishState state)
  {
    currentStage.OnStageEnd -= OnStageFinish;
    OnStageEnd?.Invoke(state);
  }
}
