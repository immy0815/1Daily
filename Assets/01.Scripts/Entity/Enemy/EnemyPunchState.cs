using System.Collections;
using System.Collections.Generic;
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
            // if (TargetIsInRange()) ;  플레이어 타격 적용 
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Quaternion targetLookRotation = Quaternion.LookRotation(enemy.GetTargetDirection());
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetLookRotation, rotationDamp * Time.deltaTime);
    }

    public override string GetNameString()
    {
        return "attackState";
    }

    bool TargetIsInRange()
    {
        float distance = (enemy.Target.transform.position - enemy.transform.position).sqrMagnitude;
        return distance < enemy.GetCurrentRange();
    }
}
