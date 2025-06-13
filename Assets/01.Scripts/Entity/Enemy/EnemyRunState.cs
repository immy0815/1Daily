using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Common.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRunState : EnemyStateBase
{
    public EnemyRunState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm)
    {
    }

    public override void Enter()
    {
        //enemy.Animator.SetBool("Run", true);
        enemy.Animator.SetBool(enemy.AnimationData.RunParameterHash, true);
    }

    public override void Exit()
    {
        enemy.Animator.SetBool("Run", false);

    }

    public override void Update()
    {
        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance && enemy.HasNoWeapon())
        {
            fsm.ChangeState(fsm.idleState);
        }
    }

    public override string GetNameString()
    {
        return "RunState";
    }
}
