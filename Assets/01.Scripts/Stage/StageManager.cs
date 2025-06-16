using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageManager
{
  private static Stage currentStage = null;
  public static Stage StartStage(int stageIndex)
  {
    var sceneName = SceneManager.GetActiveScene().name;
    if (sceneName == "GameScene")
    {
      if(currentStage != null)
      {
        currentStage.StopStage();
        Object.Destroy(currentStage.gameObject);
        currentStage = null;
      }
    }
    else SceneManager.LoadScene("GameScene");
    
    if (ResourceManager.Instance.InstantiateStage($"Stage{stageIndex}", out var obj))
    {
      var stage = obj.GetComponent<Stage>();
      obj.transform.position = Vector3.zero;
      currentStage = stage;
      stage.StartStage();
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
}
