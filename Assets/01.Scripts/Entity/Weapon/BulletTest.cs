using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    public GameObject bulletPool;
    private BulletPool bulletTest;
    private void Start()
    {
        bulletTest = bulletPool.GetComponent<BulletPool>();
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        while(true)
        {
            FireBullet();
            yield return new WaitForSeconds(1f);
        }
    }

    private void FireBullet()
    {
        GameObject bullet = bulletTest.GetBullet();

        Vector3 direction = transform.forward;

        bullet.GetComponent<Bullet>().Init(transform.position, direction);
        
    }
}
