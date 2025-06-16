using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts.Interface;
using _01.Scripts.Util;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")] 
    [SerializeField] private LayerMask hittableLayer;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private int bulletDamage = 10;
    
    [Header("Components")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private MeshRenderer meshRenderer;

    BulletPool _bulletPool;
    private bool isActive;

    private const float lifeTime = 8f;
    float elapsedTime;

    private void Awake()
    {
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!meshRenderer) meshRenderer = gameObject.GetComponent_Helper<MeshRenderer>();
    }

    private void Reset()
    {
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!meshRenderer) meshRenderer = gameObject.GetComponent_Helper<MeshRenderer>();
    }

    private void OnEnable()
    {
        isActive = true;
        
        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        meshRenderer.material.color = Color.black;
    }

    private void Update()
    {
        if (!isActive) return;
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= lifeTime) ReturnToPool();
    }

    public void Init(Vector3 position, Vector3 direction, BulletPool bulletPool)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);

        rigidBody.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        _bulletPool = bulletPool;
        elapsedTime = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (((1 << other.gameObject.layer) & hittableLayer.value) == 0) return;

        var damagable = other.GetComponent<IDamagable>();
        if (damagable == null) { ReturnToPool(); return; }
        damagable.OnTakeDamage(bulletDamage);
        ReturnToPool();
    }
    private void ReturnToPool()
    {
        isActive = false;
        gameObject.SetActive(false);
        _bulletPool.ReturnBullet(gameObject);
    } 
}
