using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private int bulletDamage = 10;
    private Rigidbody bulletRigidBody;
    private bool isActive = false;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
        bulletRigidBody.useGravity = false;
        GetComponent<MeshRenderer>().material.color = Color.black;
    }

    private void OnEnable()
    {
        isActive = true;
        bulletRigidBody.velocity = Vector3.zero;
        bulletRigidBody.angularVelocity = Vector3.zero;
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

        bulletRigidBody.velocity = direction.normalized * bulletSpeed;
    }
}
