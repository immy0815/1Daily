using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon, IShootable, IThrowable
{
    public Transform firePoint;
    public GameObject bullet;
    public GameObject bulletPoolObj;

    public void OnShoot()
    {
        BulletPool bulletpool = bulletPoolObj.GetComponent<BulletPool>();
        bullet = bulletpool.GetBullet();
        
        Vector3 direction = transform.forward;
        
        bullet.GetComponent<Bullet>().Init(transform.position, direction);
    }

    public void OnThrow(Vector3 force)
    {
        transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);

        gameObject.AddComponent<ThrownObject>().Init(weaponData.damage);
    }
}