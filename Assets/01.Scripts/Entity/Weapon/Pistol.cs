using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts;
using _01.Scripts.Util;
using UnityEngine;

public class Pistol : Weapon, IShootable
{
    [Header("Components")] 
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private BoxCollider boxCollider;
    
    [Header("Pistol Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletPoolObj;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float throwForce = 10;
    
    private void Awake()
    {
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
    }

    private void Reset()
    {
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
    }

    public void OnShoot()
    {
        BulletPool bulletpool = bulletPoolObj.GetComponent<BulletPool>();
        bullet = bulletpool.GetBullet();
        
        Vector3 direction = transform.forward;
        
        bullet.GetComponent<Bullet>().Init(firePoint.transform.position, direction);
    }

    public override void OnThrow(Vector3 direction)
    {
        transform.SetParent(null);
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        boxCollider.isTrigger = false;
        IsThrown = true;
        
        rigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        // gameObject.AddComponent<ThrownObject>().Init(WeaponData.damage);
    }

    public override void OnInteract(Transform pivot)
    {
        if (IsThrown) return;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        boxCollider.isTrigger = true;
        StartCoroutine(MoveToPivot(pivot));
    }
}