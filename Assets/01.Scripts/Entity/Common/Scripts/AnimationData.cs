using System;
using UnityEngine;

namespace _01.Scripts.Entity.Common.Scripts
{
    [Serializable] public class AnimationData
    {
        [Header("Animation Parameters on ground")]
        [SerializeField, ReadOnly] private string groundParameterName = "@Ground";
        [SerializeField, ReadOnly] private string idleParameterName = "Idle";
        [SerializeField, ReadOnly] private string walkParameterName = "Walk";
        [SerializeField, ReadOnly] private string runParameterName = "Run";
        [SerializeField, ReadOnly] private string punchParameterName = "Punch";
        
        [Header("Animation Parameters on air")]
        [SerializeField, ReadOnly] private string airParameterName = "@Air";
        [SerializeField, ReadOnly] private string jumpParameterName = "Jump";
        [SerializeField, ReadOnly] private string fallParameterName = "Fall";
        
        [Header("Animation Parameters on attack")] 
        [SerializeField, ReadOnly] private string attackParameter = "@NormalAttack";
        [SerializeField, ReadOnly] private string comboAttackIndex = "NormalCombo";
        [SerializeField, ReadOnly] private string shotParameterName = "Shot";
        
        [Header("Animation Parameter on death")]
        [SerializeField, ReadOnly] private string deathParameterName = "Dead";
        [SerializeField, ReadOnly] private string hitParameterName = "Hit";
        
        // Properties of parameter hash
        public int GroundParameterHash { get; private set; }
        public int IdleParameterHash { get; private set; }
        public int WalkParameterHash { get; private set; }
        public int RunParameterHash { get; private set; }
        public int PunchParameterHash { get; private set; }
        public int AirParameterHash { get; private set; }
        public int JumpParameterHash { get; private set; }
        public int FallParameterHash { get; private set; }
        public int AttackParameterHash { get; private set; }
        public int ComboIndexHash { get; private set; }
        public int ShotParameterHash { get; private set; }
        public int DeathParameterHash { get; private set; }
        public int HitParameterHash { get; private set; }
        
        public void Initialize()
        {
            GroundParameterHash = Animator.StringToHash(groundParameterName);
            IdleParameterHash = Animator.StringToHash(idleParameterName);
            WalkParameterHash = Animator.StringToHash(walkParameterName);
            RunParameterHash = Animator.StringToHash(runParameterName);
            PunchParameterHash = Animator.StringToHash(punchParameterName);

            AirParameterHash = Animator.StringToHash(airParameterName);
            JumpParameterHash = Animator.StringToHash(jumpParameterName);
            FallParameterHash = Animator.StringToHash(fallParameterName);
            
            AttackParameterHash = Animator.StringToHash(attackParameter);
            ComboIndexHash = Animator.StringToHash(comboAttackIndex);
            ShotParameterHash = Animator.StringToHash(shotParameterName);
            
            DeathParameterHash = Animator.StringToHash(deathParameterName);
            HitParameterHash = Animator.StringToHash(deathParameterName);
        }
    }
}