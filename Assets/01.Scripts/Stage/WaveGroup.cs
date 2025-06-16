using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveGroup : MonoBehaviour
{
  [SerializeField] private List<Enemy> enemies = new();
  public event Action OnClear;
  
  #region Unity Event
  
#if UNITY_EDITOR

  private void Reset()
  {
    for (var i = 0; i < transform.childCount; i++)
    {
      var child = transform.GetChild(i);
      if (child.gameObject.TryGetComponent<Enemy>(out var enemy))
      {
        enemies.Add(enemy);
        enemy.gameObject.SetActive(false);
      }
    }
  }

#endif

  private void Awake()
  {
    OnClear += () =>
    {
      foreach (var enemy in enemies)
      {
        enemy.OnDeath -= OnEnemyDeath;
      }
    };
  }

  #endregion
  
  #region Feature
  
  public void Spawn()
  {
    foreach (var enemy in enemies.Where(enemy => !enemy.gameObject.activeSelf))
    {
      enemy.gameObject.SetActive(true);
      enemy.OnDeath += OnEnemyDeath;
    }
  }

  private void OnEnemyDeath()
  {
    if (enemies.Any(enemy => enemy.gameObject.activeSelf))
      return;
    
    OnClear?.Invoke();
  }
  
  #endregion
}
