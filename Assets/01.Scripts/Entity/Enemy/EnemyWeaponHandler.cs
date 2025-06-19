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

    [SerializeField] private bool attackReady;
    private Coroutine attackReadyCoroutine;
    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        attackReady = false;
    }

    private void Start()
    {
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
        if (enemy.TargetPlayer.PlayerCondition.IsDead) return;
        switch (weapon)
        {
            case IShootable shootable:
                if (!attackReady)
                {
                    if(attackReadyCoroutine == null)
                        attackReadyCoroutine = StartCoroutine(WaitUntilPostureReady());
                    return;
                }
                shootable.OnShoot(enemy);
                if (!shootable.CanShoot())
                {
                    DropWeapon();
                }
                break;
            case IHittable hittable:
                hittable.OnHit();
                break;
        }
    }

    public void DropWeapon()
    {
        if (weapon) weapon.OnThrow(Vector3.down, false);
        weapon = null;
        enemy.Animator.SetBool(enemy.AnimationData.ShotParameterHash, false);
    }

    IEnumerator WaitUntilPostureReady()
    {
        yield return new WaitForSeconds(0.25f); // 정자세에서 총구 준비 자세로 향하는 시간, 일단 상수로 설정
        attackReady = true;
    }

    public void CancelReady()
    {
        attackReady = false;
        if (attackReadyCoroutine != null)
        {
            StopCoroutine(attackReadyCoroutine);
            attackReadyCoroutine = null;
        }
    }
}
