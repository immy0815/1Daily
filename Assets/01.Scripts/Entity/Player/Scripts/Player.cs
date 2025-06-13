using System;
using _01.Scripts.Entity.Common;
using _01.Scripts.Entity.Common.Scripts;
using _01.Scripts.Util;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    public class Player : MonoBehaviour
    {
        [field: Header("Animation Data")]
        [field: SerializeField] public AnimationData AnimationData { get; private set; }

        [field: Header("Components")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public PlayerController PlayerController { get; private set; }
        [field: SerializeField] public EntityCondition PlayerCondition { get; private set; }
        [field: SerializeField] public PlayerInteraction PlayerInteraction { get; private set; }
        
        private void Awake()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!PlayerController) PlayerController = gameObject.GetComponent_Helper<PlayerController>();
            if (!PlayerCondition) PlayerCondition = gameObject.GetComponent_Helper<EntityCondition>();
            if (!PlayerInteraction) PlayerInteraction = gameObject.GetComponent_Helper<PlayerInteraction>();
            
            AnimationData.Initialize();
        }

        private void Reset()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!PlayerController) PlayerController = gameObject.GetComponent_Helper<PlayerController>();
            if (!PlayerCondition) PlayerCondition = gameObject.GetComponent_Helper<EntityCondition>();
            if (!PlayerInteraction) PlayerInteraction = gameObject.GetComponent_Helper<PlayerInteraction>();
            
            AnimationData.Initialize();
        }

        // Start is called before the first frame update
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
