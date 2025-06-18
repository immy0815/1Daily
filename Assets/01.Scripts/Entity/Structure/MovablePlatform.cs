using System.Collections.Generic;
using _01.Scripts.Manager;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
  public float speed = 10f;
  public List<Vector3> positionList = new();
  public int index = 0;
  
  private void FixedUpdate()
  {
    if (Vector3.Distance(transform.position, positionList[index]) < 0.1f)
    {
      index++;
      if (index >= positionList.Count) index = 0;
    }
    
    transform.position = Vector3.MoveTowards(transform.position, positionList[index], speed * Time.unscaledDeltaTime);
  }
  
  [ContextMenu("Add Position")]
  private void AddPosition()
  {
    positionList.Add(transform.position);
  }
}
