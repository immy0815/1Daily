using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts;
using _01.Scripts.Util;
using UnityEngine;

public class Katana : Weapon, IHittable
{
    [Header("Components")] 
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private BoxCollider boxCollider;
    
    [Header("Katana Settings")]
    [SerializeField] private float throwForce = 10;
    
    protected override void Awake()
    {
        base.Awake();
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
    }

    protected override void Reset()
    {
        base.Reset();
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
    }

    public void OnHit()
    {
        Debug.Log("Katana slash");
        boxCollider.enabled = true;

        // 공격 애니메이션 이후 콜라이더 끄기
        StartCoroutine(DisableCollider(0.3f));
    }

    public override void OnThrow(Vector3 direction, bool isThrownByPlayer)
    {
        transform.SetParent(null);
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        boxCollider.isTrigger = false;
        IsThrownByPlayer = isThrownByPlayer;
        
        rigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        thrownObject.enabled = true;
    }

    public override void OnInteract(Transform pivot, bool isOwnedByPlayer)
    {
        if (IsThrownByPlayer) return;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        boxCollider.isTrigger = true;
        IsOwnedByPlayer = isOwnedByPlayer;
        StartCoroutine(MoveToPivot(pivot));
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(WeaponData.damage);
            //enemy.dropWeapon();
        }
    }
    
    private IEnumerator DisableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        boxCollider.enabled = false;
    }
}
