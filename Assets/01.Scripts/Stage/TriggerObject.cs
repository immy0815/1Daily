using _01.Scripts.Entity.Player.Scripts.Interface;
using UnityEngine;
using UnityEngine.Events;

[ AddComponentMenu( "Stage/Trigger Object" )]
public class TriggerObject : MonoBehaviour
{
  public TriggerType type;
  public UnityEvent onTrigger;
  
  private void OnTriggerEnter( Collider other )
  {
    if (other.CompareTag("Player") && type == TriggerType.TriggerEnter)
    {
      onTrigger.Invoke();
    }
  }

  public void Trigger(TriggerType type)
  {
    if (type == this.type)
    {
      onTrigger.Invoke();
    }
  }
}

public enum TriggerType
{
  TriggerEnter,
  RightClick,
  LeftClick
}