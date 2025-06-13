using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private int bulletDamage = 10;
    private Rigidbody rb;
    private bool isActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        Enemy enemy = other.GetComponent<Enemy>();
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
