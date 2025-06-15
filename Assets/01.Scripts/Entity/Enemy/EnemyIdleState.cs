using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    public EnemyIdleState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm)
    {
    }

    public override void Enter()
    {
        // enemy.Animator.SetBool(enemy.AnimationData.IdleParameterHash, true);
    }

    public override void Exit()
    {
        // enemy.Animator.SetBool(enemy.AnimationData.IdleParameterHash, false);
    }

    public override void Update()
    {
        if (!enemy.Target) return;
        
        enemy.Agent.SetDestination(enemy.Target.transform.position);
        if (!enemy.CanTouchTarget())
        {
            fsm.ChangeState(fsm.runState);
        }
        else if (!enemy.HasWeapon())  // 무기 검사 추가 필요. 거리는 되는 상황
        {
            fsm.ChangeState(fsm.punchState);
        }
    }

    public override string GetNameString()
    {
        return "IdleState";
    }
}
