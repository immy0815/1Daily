using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts.Interface;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    public Weapon Weapon => weapon;

    [SerializeField] private Transform pivot;

    private Enemy enemy;
    
    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        if (weapon is IInteractable interactable)
        {
            interactable.OnInteract(pivot);
        }
    }

    public void Init(Enemy enemyEntity)
    {
        enemy = enemyEntity;
    }

    public void Attack()
    {
        switch (weapon)
        {
            case IShootable shootable:
                shootable.OnShoot(enemy);
                break;
            case IHittable hittable:
                hittable.OnHit();
                break;
        }
    }
}
