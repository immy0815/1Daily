using System;
using _01.Scripts.Entity.Common.Scripts;
using _01.Scripts.Manager;
using _01.Scripts.Util;
using Cinemachine;
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
        [field: SerializeField] public PlayerCondition PlayerCondition { get; private set; }
        [field: SerializeField] public PlayerInteraction PlayerInteraction { get; private set; }
        [field: SerializeField] public PlayerInventory PlayerInventory { get; private set; }
        [field: SerializeField] public PlayerGravity PlayerGravity { get; private set; }
        [field: SerializeField] public Transform MainCameraTransform { get; private set; }
        [field: SerializeField] public Transform CameraPivot { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera FirstPersonCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera ThirdPersonCamera { get; private set; }
        [field: SerializeField] public CinemachineDollyCart DollyCart { get; private set; }
        [field: SerializeField] public GameObject DollyTrack { get; private set; }
        
        private PlayerStateMachine stateMachine;
        private Camera cam;
        
        private void Awake()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!PlayerController) PlayerController = gameObject.GetComponent_Helper<PlayerController>();
            if (!PlayerCondition) PlayerCondition = gameObject.GetComponent_Helper<PlayerCondition>();
            if (!PlayerInteraction) PlayerInteraction = gameObject.GetComponent_Helper<PlayerInteraction>();
            if (!PlayerInventory) PlayerInventory = gameObject.GetComponent_Helper<PlayerInventory>();
            if (!PlayerGravity) PlayerGravity = gameObject.GetComponent_Helper<PlayerGravity>();
            if (!FirstPersonCamera) FirstPersonCamera = GameObject.Find("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
            if (!ThirdPersonCamera) ThirdPersonCamera = GameObject.Find("ThirdPersonCamera").GetComponent<CinemachineVirtualCamera>();
            if (!DollyCart) DollyCart = GameObject.Find("Dolly Cart").GetComponent<CinemachineDollyCart>();
            if (!DollyTrack) DollyTrack = GameObject.Find("Dolly Track");
            if (!CameraPivot) CameraPivot = gameObject.FindObjectAndGetComponentInChildren_Helper<Transform>("CameraPivot");
            
            AnimationData.Initialize();
            PlayerInteraction.Init(this);
        }

        private void Reset()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!PlayerController) PlayerController = gameObject.GetComponent_Helper<PlayerController>();
            if (!PlayerCondition) PlayerCondition = gameObject.GetComponent_Helper<PlayerCondition>();
            if (!PlayerInteraction) PlayerInteraction = gameObject.GetComponent_Helper<PlayerInteraction>();
            if (!PlayerInventory) PlayerInventory = gameObject.GetComponent_Helper<PlayerInventory>();
            if (!PlayerGravity) PlayerGravity = gameObject.GetComponent_Helper<PlayerGravity>();
            if (!FirstPersonCamera) FirstPersonCamera = GameObject.Find("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
            if (!ThirdPersonCamera) ThirdPersonCamera = GameObject.Find("ThirdPersonCamera").GetComponent<CinemachineVirtualCamera>();
            if (!DollyCart) DollyCart = GameObject.Find("Dolly Cart").GetComponent<CinemachineDollyCart>();
            if (!DollyTrack) DollyTrack = GameObject.Find("Dolly Track");
            if (!CameraPivot) CameraPivot = gameObject.FindObjectAndGetComponentInChildren_Helper<Transform>("CameraPivot");
            
            AnimationData.Initialize();
            PlayerInteraction.Init(this);
        }

        // Start is called before the first frame update
        private void Start()
        {
            FirstPersonCamera.Follow = CameraPivot;
            ThirdPersonCamera.LookAt = CameraPivot;
            cam = Camera.main;
            MainCameraTransform = cam?.transform;
            
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

        private void LateUpdate()
        {
            stateMachine.LateUpdate();
        }

        private void OnDeath()
        {
            FirstPersonCamera.Priority = 5; 
            ThirdPersonCamera.Priority = 10; 
            cam.cullingMask |= 1 << gameObject.layer;
            DollyTrack.transform.SetPositionAndRotation(new Vector3(transform.position.x, DollyTrack.transform.position.y, transform.position.z), transform.rotation);
            
            Animator.SetTrigger(AnimationData.DeathParameterHash);
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Death, 0.5f);
            
            DollyCart.m_Speed = 3f;
        }
    }
}
