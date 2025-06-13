using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon, IShootable, IThrowable
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public void OnShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // 오브젝트풀링으로 총알 구현되면 변경
        bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * 20f;
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