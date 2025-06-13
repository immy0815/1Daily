using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponType;
    public int damage;
    public GameObject prefab;
}
