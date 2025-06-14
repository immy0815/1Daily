using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanon : MonoBehaviour
{
    public GameObject bulletPrefab;

    private void Start()
    {
        StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        while (true)
        {
            GameObject bulletGO = Instantiate(bulletPrefab);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.Init(transform.position+transform.forward, transform.forward);
            yield return new WaitForSeconds(2f);
        }
    }
}
