using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Weapon, IHittable, IThrowable
{
    [SerializeField] private Collider katanaCollider;
    
    public void OnHit()
    {
        Debug.Log("Katana slash");
        katanaCollider.enabled = true;

        // 공격 애니메이션 이후 콜라이더 끄기
        StartCoroutine(DisableCollider(0.3f));
        
    }

    public void OnThrow(Vector3 force)
    {
        transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);

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
}
