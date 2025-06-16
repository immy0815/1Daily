using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts.Interface;
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    [Header("Object Settings")] 
    [SerializeField] private LayerMask hittableLayer;
    
    private int damage;
    private bool hasAppliedDamage;
    private bool isReady;

    public void Init(int damage)
    {
        this.damage = damage;
        hasAppliedDamage = false;
        isReady = true;
    }

    private void OnCollisionStay(Collision other)
    {
        if (hasAppliedDamage || !isReady) return;
        if (((1 << other.gameObject.layer) & hittableLayer.value) == 0) return;

        var damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable == null) { Destroy(gameObject); return; }
        damagable.OnTakeDamage(damage);
        Destroy(gameObject);
    }
}
