using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyStateBase
{
    public float elapsedTime;
    public EnemyHitState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm)
    {
    }

    public override void Enter()
    {
        elapsedTime = 0;
        enemy.Animator.SetTrigger("Hit");
        enemy.IsHit = false;
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        // 애니메이션 시간에 기댈지, 데이터 경직 시간에 기댈지 결정 필요
        
        // ---------- 애니메이션 종료시 경직 풀리는 코드 ----------
        // AnimatorStateInfo stateInfo = enemy.Animator.GetCurrentAnimatorStateInfo(0);
        // float progress = stateInfo.normalizedTime;
        //
        // if(progress >= 1f) fsm.ChangeState(fsm.idleState);
        
        // ---------- SO 데이터의 경직 시간이 지나면 경직 풀리는 코드 -----------
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= enemy.EnemyData.StiffnessTime)
        {
            fsm.ChangeState(fsm.idleState);
        }
    }

    public override string GetNameString()
    {
        return "HitState";
    }
}
