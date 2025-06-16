using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Common.Scripts;
using _01.Scripts.Entity.Player.Scripts;
using _01.Scripts.Entity.Player.Scripts.Interface;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    public AnimationData AnimationData { get; private set; }
    
    [Header("Awake Auto-Set Components")]
    [SerializeField]private Animator animator;
    public Animator Animator => animator;
    
    [SerializeField] private EnemyFSM fsm;
    public EnemyFSM FSM => fsm;
     
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    [SerializeField] private EnemyWeaponHandler weaponHandler;
    public EnemyWeaponHandler WeaponHandler => weaponHandler;

    [SerializeField] private EnemySightSensor sightSensor;
    public EnemySightSensor SightSensor => sightSensor;
    
    private CharacterController characterController;

    [Header("Stats")] 
    [SerializeField] private EnemyData enemyData;
    public EnemyData EnemyData => enemyData;
    [SerializeField] private int currentHP;

    [Header("Runtime Info")] 
    [SerializeField] private Transform target;
    public Transform Target => target;
    [SerializeField] private Player targetPlayer;
    public Player TargetPlayer => targetPlayer;
    
    public bool IsHit { get; set; }
    public bool IsDead { get; private set; }

    public event Action OnDeath;

    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();
        animator = GetComponentInChildren<Animator>();
        fsm = GetComponent<EnemyFSM>();
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        weaponHandler = GetComponent<EnemyWeaponHandler>();
        sightSensor = GetComponentInChildren<EnemySightSensor>();

        IsDead = false;
        currentHP = enemyData.HP;
    }

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
        fsm = GetComponent<EnemyFSM>();
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        weaponHandler = GetComponent<EnemyWeaponHandler>();
    }

    private void Start()
    {
        weaponHandler.Init(this);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;

        if (!target)
        {
            targetPlayer = null;
        }
        else if (!targetPlayer || targetPlayer.transform != target)
        {
            targetPlayer = target.GetComponent<Player>();
        }
    }

    public float DistanceToTargetSQR()
    {
        return (target.position - transform.position).sqrMagnitude;
    }
    
    public bool HasWeapon()
    {
        return weaponHandler.Weapon;
    }

    public void OnTakeDamage(int damage)
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
        
        OnDeath?.Invoke();
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
