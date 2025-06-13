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

    [Header("Stats")]
    // SO
    [SerializeField] private int currentHP = 3;

    [Header("Runtime Info")] 
    [SerializeField] private Transform target;
    public Transform Target => target;

    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();
        animator = GetComponentInChildren<Animator>();
        fsm = GetComponent<EnemyFSM>();
        agent = GetComponent<NavMeshAgent>();
        
        Agent.SetDestination(Target.position);
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
