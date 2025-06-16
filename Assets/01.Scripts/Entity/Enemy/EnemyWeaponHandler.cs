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

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        if (weapon is IInteractable interactable)
        {
            interactable.OnInteract(pivot);
        }
    }

    public void Attack()
    {
        switch (weapon)
        {
            case IShootable shootable:
                shootable.OnShoot();
                break;
            case IHittable hittable:
                hittable.OnHit();
                break;
        }
    }
}
