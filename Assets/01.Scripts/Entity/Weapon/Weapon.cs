using UnityEngine;

public abstract class Weapon : MonoBehaviour, IThrowable
{
    public WeaponData weaponData;
    public virtual void Equip() { }

    public virtual void Unequip() { }

    public void OnThrow(Vector3 force) { }
}