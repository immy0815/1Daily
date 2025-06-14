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
        [field: SerializeField] public PlayerInventory PlayerInventory { get; private set; }
        [field: SerializeField] public PlayerGravity PlayerGravity { get; private set; }
        [field: SerializeField] public Transform MainCameraTransform { get; private set; }
        
        private PlayerStateMachine stateMachine;
        
        private void Awake()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!PlayerController) PlayerController = gameObject.GetComponent_Helper<PlayerController>();
            if (!PlayerCondition) PlayerCondition = gameObject.GetComponent_Helper<EntityCondition>();
            if (!PlayerInteraction) PlayerInteraction = gameObject.GetComponent_Helper<PlayerInteraction>();
            if (!PlayerInventory) PlayerInventory = gameObject.GetComponent_Helper<PlayerInventory>();
            if (!PlayerGravity) PlayerGravity = gameObject.GetComponent_Helper<PlayerGravity>();
            
            AnimationData.Initialize();
            PlayerInteraction.Init(this);
        }

        private void Reset()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!PlayerController) PlayerController = gameObject.GetComponent_Helper<PlayerController>();
            if (!PlayerCondition) PlayerCondition = gameObject.GetComponent_Helper<EntityCondition>();
            if (!PlayerInteraction) PlayerInteraction = gameObject.GetComponent_Helper<PlayerInteraction>();
            if (!PlayerInventory) PlayerInventory = gameObject.GetComponent_Helper<PlayerInventory>();
            if (!PlayerGravity) PlayerGravity = gameObject.GetComponent_Helper<PlayerGravity>();
            
            AnimationData.Initialize();
            PlayerInteraction.Init(this);
        }

        // Start is called before the first frame update
        private void Start()
        {
            MainCameraTransform = Camera.main?.transform;
            
            Cursor.lockState = CursorLockMode.Locked;
            stateMachine = new PlayerStateMachine(this);
            stateMachine.ChangeState(stateMachine.IdleState);

            PlayerCondition.OnDeath += OnDeath;
        }

        private void FixedUpdate()
        {
            stateMachine.PhysicsUpdate();
        }

        private void Update()
        {
            stateMachine.HandleInput();
            stateMachine.Update();
        }
        
        private void OnDeath()
        {
            Animator.SetTrigger(AnimationData.DeathParameterHash);
        }
    }
}
