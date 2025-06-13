using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponType;
    public float damage;
    public GameObject prefab;
}
