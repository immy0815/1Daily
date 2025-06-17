using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ AddComponentMenu( "Stage/WaveGroup" )]
public class WaveGroup : MonoBehaviour
{
  [SerializeField, Tooltip("웨이브의 적 목록입니다. Reset 시 자동으로 설정합니다.")] private List<Enemy> enemies = new();
  
  /// <summary>
  /// 웨이브가 클리어됬을 때 호출하는 이벤트입니다.
  /// </summary>
  public event Action OnClear;
  
  #region Unity Event
  
#if UNITY_EDITOR

  /// <summary>
  /// 적을 자동으로 설정하는 유니티 에디터 전용 메서드입니다.
  /// </summary>
  public void Reset()
  {
    enemies.Clear();
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

  #endregion
  
  #region Feature
  
  /// <summary>
  /// 활성화상태가 아닌 적들을 활성화합니다.
  /// </summary>
  public void Spawn()
  {
    foreach (var enemy in enemies.Where(enemy => !enemy.gameObject.activeSelf))
    {
      enemy.gameObject.SetActive(true);
      enemy.OnDeath += OnEnemyDeath;
      continue;

      void OnEnemyDeath()
      {
        enemy.OnDeath -= OnEnemyDeath;
        foreach (var enemy in enemies)
        {
          if (!enemy.IsDead)
          {
            return;
          }
        }

        OnClear?.Invoke();
      }
    }
  }
  
  #endregion
}
