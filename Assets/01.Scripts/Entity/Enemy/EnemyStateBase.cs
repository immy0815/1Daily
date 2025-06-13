using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Common.Scripts.Interface;
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

    public abstract void Enter();
    public void HandleInput()
    {
 
    }

    public void PhysicsUpdate()
    {

    }

    public abstract void Exit();

    public abstract void Update();
    public abstract string GetNameString();
}
