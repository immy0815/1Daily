using UnityEngine;

[ AddComponentMenu( "Stage/Stage Clear Object" )]
public class StageClearTrigger : TriggerObject
{
  private void Awake()
  {
    onTrigger.AddListener(StageManager.StartNextStageStatic);
  }
}
