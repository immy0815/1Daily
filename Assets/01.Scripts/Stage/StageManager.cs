using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class StageManager
{
  private static Stage currentStage = null;
  
  public static event Action<Stage> OnStageStart;
  public static event Action<Stage> OnStageEnd;
  
  public static Stage StartStage(int stageIndex)
  {
    var sceneName = SceneManager.GetActiveScene().name;
    if (sceneName == "GameScene") StopStage();
    else SceneManager.LoadScene("GameScene");
    
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

  public static void StopStage()
  {
    if(currentStage)
    {
      currentStage.StopStage();
      Object.Destroy(currentStage.gameObject);
      currentStage = null;
    }
  }

  private static void OnStageFinish(StageFinishType type)
  {
    currentStage.OnStageEnd -= OnStageFinish;
    OnStageEnd?.Invoke(currentStage);
  }
}
