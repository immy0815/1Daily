using System;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    public class PlayerInventory : MonoBehaviour
    {
        [field: Header("Current Weapon")]
        [field: SerializeField] public Weapon CurrentWeapon { get; private set; }

        private void Start()
        {
            CurrentWeapon = null;
        }

        // Methods
        public void EquipWeapon(Weapon weapon) { CurrentWeapon = weapon; }
        // public void DropWeapon() { if (CurrentWeapon) CurrentWeapon.OnThrow(); }
    }
}