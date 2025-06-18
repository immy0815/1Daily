using System;
using UnityEngine.SceneManagement;

public class SceneMoveTrigger : TriggerObject
{
  public string sceneName;
  private void Awake()
  {
    onTrigger.AddListener(() => SceneManager.LoadScene(sceneName));;
  }
}
