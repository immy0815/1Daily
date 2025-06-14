using System;
using _01.Scripts.Entity.Player.Scripts.Interface;
using JetBrains.Annotations;
using UnityEngine;

namespace _01.Scripts.Entity.Common.Scripts
{
    public class EntityCondition : MonoBehaviour, IDamagable
    {
        [field: Header("Basic Entity Settings")]
        [field: SerializeField] public int Health { get; private set; } = 1;
        [field: SerializeField] public int Damage { get; private set; } = 1;
        [field: SerializeField] public bool IsDead { get; private set; }

        [field: Header("Entity Physics Settings")]
        [field: SerializeField] public float Speed { get; private set; } = 5f;
        [field: SerializeField] public float WalkSpeedThreshold { get; private set; } = 0.01f;
        [field: SerializeField] public float RunSpeedThreshold { get; private set; } = 3f; 
        [field: SerializeField] public float JumpForce { get; private set; } = 5f;
        [field: SerializeField] public float RotationalDamping { get; private set; } = 3f;
        
        // Action events
        [CanBeNull] public event Action OnDamage, OnDeath;

        public void OnTakeDamage(int damage)
        {
            if (IsDead) return;
            Health -= damage;
            OnDamage?.Invoke();
            
            if(Health <= 0) OnDead();
        }

        private void OnDead()
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}
