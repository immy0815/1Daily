using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Weapon, IHittable, IThrowable
{
    [SerializeField] private Collider katanaCollider;
    private Rigidbody katanaRigidbody;

    private void Awake()
    {
        katanaRigidbody = GetComponent<Rigidbody>();
    }

    public void OnHit()
    {
        katanaCollider.enabled = true;

        // 공격 애니메이션시간 이후 콜라이더 끄기 -> 애니메이션동안만 콜라이더 On해서 적이랑 충돌처리하려고 했습니다.
        StartCoroutine(DisableCollider(0.3f));
    }

    public void OnThrow(Vector3 force)
    {
        transform.parent = null;
        katanaRigidbody.isKinematic = false;
        katanaRigidbody.AddForce(force, ForceMode.Impulse);
        gameObject.AddComponent<ThrownObject>().Init(weaponData.damage);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(weaponData.damage);
            //enemy.dropWeapon();
        }
    }
    
    private IEnumerator DisableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        katanaCollider.enabled = false;
    }

    public virtual void Equip(Transform transform)
    {
        katanaRigidbody.isKinematic = true;
        katanaRigidbody.velocity = Vector3.zero;
        katanaRigidbody.angularVelocity = Vector3.zero;
        
        if (katanaCollider != null) katanaCollider.enabled = false;

        transform.SetParent(transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void Unequip()
    {
        transform.SetParent(null);
        if (katanaCollider != null) katanaCollider.enabled = true;
        katanaRigidbody.isKinematic = false;
    }
}
