using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    private float damage;
    private bool hasAppliedDamage;

    public void Init(float damage)
    {
        this.damage = damage;
        hasAppliedDamage = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (hasAppliedDamage) return;
        
        /*
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.takeDamage(damage);
            enemy.dropWeapon();
            hasAppliedDamage = true;
        }
        */
        
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); 
        }
    }
}
