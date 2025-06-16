using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanon : MonoBehaviour
{
    //public GameObject bulletPrefab;
    [SerializeField] BulletPool bulletPool;

    private void Awake()
    {
        if (!TryGetComponent(out BulletPool _bulletPool))
        {
            Debug.LogWarning("BulletPool component not found");
        }
        else
        {
            bulletPool = _bulletPool;
        }
    }

    private void Start()
    {
        StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        while (true)
        {
            GameObject bulletGO = bulletPool.GetBullet();
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Init(transform.position + transform.forward / 2, transform.forward, bulletPool);
            yield return new WaitForSeconds(2f);
        }
    }
}
