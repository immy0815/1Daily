using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Common.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public AnimationData AnimationData { get; private set; }
    
    [Header("Awake Auto-Set Components")]
    [SerializeField]private Animator animator;
    public Animator Animator => animator;
    
    [SerializeField] private EnemyFSM fsm;
    public EnemyFSM FSM => fsm;
    
     
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    private CharacterController characterController;

    [Header("Stats")] 
    [SerializeField] private EnemyData enemyData;
    public EnemyData EnemyData => enemyData;
    [SerializeField] private int currentHP;

    [Header("Runtime Info")] 
    [SerializeField] private Transform target;
    public Transform Target => target;
    public bool IsHit { get; set; }
    public bool IsDead { get; private set; }


    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();
        animator = GetComponentInChildren<Animator>();
        fsm = GetComponent<EnemyFSM>();
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();

        IsDead = false;
        currentHP = enemyData.HP;
    }

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
        fsm = GetComponent<EnemyFSM>();
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public float DistanceToTargetSQR()
    {
        return (target.position - transform.position).sqrMagnitude;
    }

    public float GetCurrentRange()
    {
        return 3;
    }

    public bool HasNoWeapon()
    {
        return true;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        if (currentHP != 0) IsHit = true;
        else Die();
    }

    private void Die()
    {
        IsDead = true;
        Animator.SetTrigger(AnimationData.DeathParameterHash);
        agent.enabled = false;
        fsm.enabled = false;
        characterController.enabled = false;
        enabled = false;
    }

    public Vector3 GetTargetDirection()
    {
        return (target.position - transform.position).normalized;
    }

    public bool CanTouchTarget()
    {
       return Target && DistanceToTargetSQR() < Mathf.Pow(Agent.stoppingDistance, 2);
    }
}
