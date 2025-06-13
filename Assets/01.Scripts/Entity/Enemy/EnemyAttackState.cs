using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    public EnemyAttackState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm)
    {
    }


    public override void Enter()
    {
        enemy.Animator.SetBool("Attack", true);

    }

    public override void Exit()
    {
        enemy.Animator.SetBool("Attack", false);
    }

    public override void Update()
    {

    }

    public override string GetNameString()
    {
        return "attackState";
    }
}
