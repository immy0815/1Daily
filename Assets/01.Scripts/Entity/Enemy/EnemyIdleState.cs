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
        if (enemy.Target && enemy.Agent.remainingDistance > enemy.Agent.stoppingDistance)
        {
            enemy.Agent.SetDestination(enemy.Target.position);
            fsm.ChangeState(fsm.runState);
        }
    }

    public override string GetNameString()
    {
        return "IdleState";
    }
}
