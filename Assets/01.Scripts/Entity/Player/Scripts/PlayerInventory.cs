using System;
using System.Collections;
using _01.Scripts.Manager;
using _01.Scripts.Util;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    public class PlayerInventory : MonoBehaviour
    {
        [field: Header("Weapon Settings")]
        [field: SerializeField] public Weapon CurrentWeapon { get; private set; }
        [field: SerializeField] public Transform CameraPivot { get; private set; }
        [field: SerializeField] public Transform WeaponPivot { get; private set; }

        public Coroutine ThrowCoroutine { get; private set; }
        
        private void Awake()
        {
            if (!CameraPivot) CameraPivot = GameObject.Find("CameraPivot").transform;
            if (!WeaponPivot) WeaponPivot = GameObject.Find("WeaponPivot").transform;
        }

        private void Reset()
        {
            if (!CameraPivot) CameraPivot = GameObject.Find("CameraPivot").transform;
            if (!WeaponPivot) WeaponPivot = GameObject.Find("WeaponPivot").transform;
        }

        private void Start()
        {
            CurrentWeapon = null;
            ThrowCoroutine = null;
        }

        // Methods
        public void OnEquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            weapon.OnInteract(WeaponPivot);
        }
        public void OnDropWeapon(Vector3 direction) 
        {
            if (CurrentWeapon)
            {
                ThrowCoroutine = StartCoroutine(ChangeTimeScaleForSeconds(0.5f));
                CurrentWeapon.OnThrow(direction);
            }
            CurrentWeapon = null;
        }

        public void ResetThrowCoroutine()
        {
            if (ThrowCoroutine == null) return;
            StopCoroutine(ThrowCoroutine);
            ThrowCoroutine = null;
        }
        
        private IEnumerator ChangeTimeScaleForSeconds(float timeDuration)
        {
            var time = 0f;
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Throw, 1);
            while (time < timeDuration)
            {
                time += Time.unscaledDeltaTime;
                var t = time / timeDuration;
                TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Throw,Mathf.Lerp(Time.timeScale, 0.01f, t));
                yield return null;
            }
            ThrowCoroutine = null;
        }
    }
}