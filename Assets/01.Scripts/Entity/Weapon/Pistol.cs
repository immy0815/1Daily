using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon, IShootable, IThrowable
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private int bulletCount;
    
    private Rigidbody PistolRigidbody;
    private Collider PistolCollider;

    private void Awake()
    {
        PistolRigidbody = GetComponent<Rigidbody>();
        PistolCollider = GetComponent<Collider>();
    }

    public void OnShoot()
    {
        Vector3 direction = transform.forward;
        BulletPool.Instance.GetBullet().GetComponent<Bullet>().Init(firePoint.transform.position, direction);
    }

    public void OnThrow(Vector3 force)
    {
        transform.parent = null;
        PistolRigidbody.isKinematic = false;
        PistolRigidbody.AddForce(force, ForceMode.Impulse);
        gameObject.AddComponent<ThrownObject>().Init(weaponData.damage);
    }

    public virtual void Equip(Transform transform)
    {
        PistolRigidbody.isKinematic = true;
        PistolRigidbody.velocity = Vector3.zero;
        PistolRigidbody.angularVelocity = Vector3.zero;
        
        if (PistolCollider != null) PistolCollider.enabled = false;

        transform.SetParent(transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void Unequip()
    {
        transform.SetParent(null);
        if (PistolCollider != null) PistolCollider.enabled = true;
        PistolRigidbody.isKinematic = false;
    }
}