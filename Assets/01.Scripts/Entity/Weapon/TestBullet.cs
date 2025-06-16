using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    // 총알 테스트 위해서 만든 스크립트, bulletPool 필요합니다.
    
    private GameObject bullet;
    public GameObject bulletPool;
    private BulletPool bulletPool2;
    public GameObject firePoint;
    private void Start()
    {
        BulletPool bulletPool2 = bulletPool.GetComponent<BulletPool>();
        
        bullet = bulletPool2.GetBullet();

        Vector3 direction = transform.forward;
        
        bullet.GetComponent<Bullet>().Init(firePoint.transform.position, direction, false);
    }
}
