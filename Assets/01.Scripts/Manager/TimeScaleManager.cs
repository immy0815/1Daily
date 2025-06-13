using System;
using System.Collections;
using UnityEngine;

namespace _01.Scripts.Manager
{
    public enum PriorityType
    {
        Move,
        Attack,
        Jump,
    }
    
    public class TimeScaleManager : MonoBehaviour
    {
        // Properties
        [field: Header("TimeScale Values")]
        [field: SerializeField] public PriorityType PreviousUpdateType { get; private set; } = PriorityType.Move;
        [field: SerializeField] public float TargetTimeScale { get; private set; } = 0.01f;
        [field: SerializeField] public float CurrentTimeScale { get; private set; }
        
        // Fields
        private Coroutine timeScaleCoroutine;
        
        // Singleton
        public static TimeScaleManager Instance { get; private set; }
        
        private void Awake()
        {
            if (!Instance) Instance = this;
            else { if (Instance != this) Destroy(gameObject); }
        }

        private void Start()
        {
            Time.timeScale = TargetTimeScale;
            CurrentTimeScale = Time.timeScale;
        }

        public void ChangeTimeScale(PriorityType type, float timeScale)
        {
            if (PreviousUpdateType == PriorityType.Jump && type == PriorityType.Attack) return;
            if (TargetTimeScale < timeScale || PreviousUpdateType == type) TargetTimeScale = timeScale;
            Time.timeScale = TargetTimeScale;
            
            PreviousUpdateType = type;
        }
    }
}