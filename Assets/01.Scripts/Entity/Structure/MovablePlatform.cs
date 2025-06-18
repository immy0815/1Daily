using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
  public float speed = 1f;
  public MoveType moveType = MoveType.Fixed;
  public TimeScaleMode timeScaleMode = TimeScaleMode.Scaled;
  public List<Vector3> positionList = new();
  public int index = 0;
  
  private void FixedUpdate()
  {
    if (Vector3.Distance(transform.position, positionList[index]) < 0.1f)
    {
      index++;
      if (index >= positionList.Count) index = 0;
    }
    
    var timeScale = timeScaleMode == TimeScaleMode.Scaled ? Time.deltaTime : Time.unscaledDeltaTime;

    if (moveType == MoveType.Lerp)
    {
      transform.position = Vector3.Lerp(transform.position, positionList[index], speed * timeScale);
    }
    else if (moveType == MoveType.Fixed)
    {
      transform.position += (positionList[index] - transform.position).normalized * (speed * timeScale);
    }
  }
  
  [ContextMenu("Add Current Position")]
  private void AddPosition()
  {
    positionList.Add(transform.position);
  }
}

public enum MoveType
{
  Lerp, Fixed
}

public enum TimeScaleMode
{
  Scaled, Unscaled
}
