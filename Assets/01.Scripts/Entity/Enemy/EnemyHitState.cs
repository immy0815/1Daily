using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyStateBase
{
    public EnemyHitState(Enemy enemy, EnemyFSM fsm) : base(enemy, fsm)
    {
    }

    public override void Enter()
    {
        enemy.Animator.SetTrigger("Hit");
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        AnimatorStateInfo stateInfo = enemy.Animator.GetCurrentAnimatorStateInfo(0);
        float progress = stateInfo.normalizedTime;
        
        if(progress >= 1f) fsm.ChangeState(fsm.idleState);
    }

    public override string GetNameString()
    {
        return "HitState";
    }
}
