using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Weapon, IHittable, IThrowable
{
    public void OnHit()
    {
        Debug.Log("Katana slash");
    }

    public void OnThrow(Vector3 force)
    {
        transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);

        gameObject.AddComponent<ThrownObject>().Init(weaponData.damage);
    }
}
