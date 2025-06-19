using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts;
using UnityEngine;

public class EnemyPunchState : EnemyStateBase
{
    public float elapsedTime;
    private const float rotationDamp = 3f; // NavMesh가 기본적인 로테이션을 해줘서 일단 내부에 설정
    public EnemyPunchState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm)
    {
    }


    public override void Enter()
    {
        enemy.Animator.SetBool("Punch", true);
        elapsedTime = 0;
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("Punch", false);
    }

    public override void Update()
    {
        float punchDuration = enemy.EnemyData.PunchDuration;
        float punchApplyTime = enemy.EnemyData.PunchApplyTime01;
        
        float prevElapsedTime = elapsedTime;
        elapsedTime += Time.deltaTime;
        
        float prevElapsedTime01 = prevElapsedTime / punchDuration;
        float elapsedTime01 = elapsedTime / punchDuration;
        
        if (elapsedTime > punchDuration)
        {
            fsm.ChangeState(fsm.idleState);
        }
        else if (prevElapsedTime01 < punchApplyTime && elapsedTime01 > punchApplyTime)
        {
             // 플레이어에게 데미지 적용
             // 플레이어 레이캐스트 탐지 시 실시간 GetComponent를 안 하기 위해 여기서 컴포넌트를 불러옴
             if (TargetIsInRange() && enemy.TargetPlayer)
                 enemy.TargetPlayer.PlayerCondition.OnTakeDamage(1);
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (!enemy.Target) return;
        Quaternion targetLookRotation = Quaternion.LookRotation(enemy.GetTargetDirection());
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetLookRotation, rotationDamp * Time.deltaTime);
    }

    public override string GetNameString()
    {
        return "attackState";
    }

    bool TargetIsInRange()
    {
        if (!enemy.Target)
            return false;
        float distance = (enemy.Target.transform.position - enemy.transform.position).sqrMagnitude;
        return distance < enemy.EnemyData.BaseRange;
    }
}
