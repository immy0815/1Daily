using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateBase : IState
{
    protected Enemy enemy;
    protected EnemyFSM fsm;

    protected EnemyStateBase(Enemy enemy, EnemyFSM fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public void ChangeState()
    {
        
    }

    public abstract void Enter();
    public abstract void Exit();

    public abstract void Update();
    public abstract string GetNameString();
}
