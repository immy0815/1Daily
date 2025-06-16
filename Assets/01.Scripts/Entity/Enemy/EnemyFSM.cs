using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    private Enemy enemy;
    private EnemyStateBase currentState;
    [SerializeField] string currentStateName;   // 인스펙터 관측용

    public EnemyIdleState idleState;
    public EnemyRunState runState;      // runState 안에서 무기 소지시 공격
    public EnemyPunchState punchState;
    public EnemyHitState hitState;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        idleState = new EnemyIdleState(enemy, this);
        runState = new EnemyRunState(enemy, this);
        punchState = new EnemyPunchState(enemy, this);
        hitState = new EnemyHitState(enemy, this);
        
        ChangeState(idleState);
    }

    private void Update()
    {
        if (enemy.IsHit)
        {
            ChangeState(hitState);
        }
        HandleLightWeapon();
        
        currentState.Update();
    }

    // 움직이면서 공격 가능한 무기
    private void HandleLightWeapon()
    {
        if (currentState != idleState && currentState != runState) return;
        
        if (enemy.WeaponHandler.Weapon is IShootable)
        {
            if (enemy.Target)
            {
                enemy.Animator.SetBool(enemy.AnimationData.ShotParameterHash, true);
                enemy.WeaponHandler.Attack();
            }
            else
            {
                enemy.Animator.SetBool(enemy.AnimationData.ShotParameterHash, false);
                enemy.WeaponHandler.CancelReady();
            }
        }
    }

    public void ChangeState(EnemyStateBase newState)
    {
        currentState?.Exit();
        currentState = newState;
        newState?.Enter();
        
        currentStateName = newState?.GetNameString();
    }
}
