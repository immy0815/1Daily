using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private float bulletSpeed = 200f;
    private bool isActive = false;
    private int bulletDamage = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<MeshRenderer>().material.color = Color.black;
    }

    private void OnEnable()
    {
        isActive = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (!isActive) return;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;

        
         Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }
        

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    public void Init(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        rb.velocity = direction.normalized * bulletSpeed;
    }
}
