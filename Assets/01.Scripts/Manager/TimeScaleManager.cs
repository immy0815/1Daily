using System;
using System.Collections;
using UnityEngine;

namespace _01.Scripts.Manager
{
    public enum PriorityType
    {
        Move,
        Attack,
        Throw,
        Jump,
    }
    
    public class TimeScaleManager : MonoBehaviour
    {
        // Properties
        [field: Header("TimeScale Values")]
        [field: SerializeField] public PriorityType PreviousUpdateType { get; private set; } = PriorityType.Move;
        [field: SerializeField] public float TargetTimeScale { get; private set; } = 0.01f;
        
        // Fields
        private Coroutine timeScaleCoroutine;
        private float originalFixedDeltaTime;
        
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
            originalFixedDeltaTime = Time.fixedDeltaTime;
        }

        public void ChangeTimeScale(PriorityType type, float timeScale)
        {
            if (PreviousUpdateType == PriorityType.Jump && type is PriorityType.Attack or PriorityType.Throw) return;
            
            
            if (Mathf.Approximately(TargetTimeScale, timeScale)) return; 
            // Debug.Log($"Time Scale Changed from {TargetTimeScale} to {timeScale}");
            TargetTimeScale = timeScale;
            PreviousUpdateType = type;
            
            Time.timeScale = TargetTimeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
        }
    }
}