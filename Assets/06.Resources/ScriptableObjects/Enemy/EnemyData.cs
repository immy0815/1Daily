using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Enemy Data", menuName = "Scriptable/Enemy")]
public class EnemyData : ScriptableObject
{
    [field:SerializeField] public int HP { get; private set; }
    [field:SerializeField] public float BaseRange { get; private set; } // 무기 미장착 시 사거리. agent StopDistance보다 높게 설정할것
    [field: SerializeField] public float PunchDuration { get; private set; }
    [field: SerializeField] public float PunchApplyTime01 { get; private set; }
    [field: SerializeField] public float StiffnessTime { get; private set; } // 경직 시간
}
