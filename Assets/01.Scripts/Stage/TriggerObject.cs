using _01.Scripts.Entity.Player.Scripts.Interface;
using UnityEngine;
using UnityEngine.Events;

[ AddComponentMenu( "Stage/Trigger Object" )]
public class TriggerObject : MonoBehaviour, IInteractable
{
  public UnityEvent onTrigger = new();
  
  public void OnInteract() => onTrigger.Invoke();

  public void OnInteract(Transform pivot, bool isOwnedByPlayer = false) { }
}
