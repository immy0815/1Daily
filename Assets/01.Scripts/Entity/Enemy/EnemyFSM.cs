using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    private Enemy enemy;
    private EnemyStateBase currentState;
    [SerializeField] string currentStateName;

    public EnemyIdleState idleState;
    public EnemyRunState runState;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        idleState = new EnemyIdleState(enemy, this);
        runState = new EnemyRunState(enemy, this);
        
        ChangeState(idleState);
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangeState(EnemyStateBase newState)
    {
        currentState?.Exit();
        currentState = newState;
        newState?.Enter();
        
        currentStateName = newState?.GetNameString();
    }
}
