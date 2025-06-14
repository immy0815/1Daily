using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    public Weapon Weapon => weapon;

    public void Attack()
    {
        switch (weapon)
        {
            case IShootable shootable:
                shootable.OnShoot();
                break;
            case IHittable hittable:
                hittable.OnHit();
                break;
        }
    }
}
