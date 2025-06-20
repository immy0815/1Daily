using System;
using System.Collections;
using _01.Scripts.Entity.Common.Scripts;
using _01.Scripts.Manager;
using _01.Scripts.Util;
using Cinemachine;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    public class Player : MonoBehaviour
    {
        [field: Header("Dolly Card Settings")]
        [field: SerializeField] public float CartSpeed { get; private set; } = 3f;
        
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

        [field: SerializeField] private Transform nectTr;
        [field: SerializeField] private Transform legLTr;
        [field: SerializeField] private Transform legRTr;
        
        private PlayerStateMachine stateMachine;
        private Coroutine deathCoroutine;
        
        public Camera cam { get; private set; }
        
        private void Awake()
        {
            if (!Animator) Animator = gameObject.GetComponentInChildren_Helper<Animator>();
            if (!CharacterController) CharacterController = gameObject.GetComponent_Helper<CharacterController>();
            if (!PlayerController) PlayerController = gameObject.GetComponent_Helper<PlayerController>();
            if (!PlayerCondition) PlayerCondition = gameObject.GetComponent_Helper<PlayerCondition>();
            if (!PlayerInteraction) PlayerInteraction = gameObject.GetComponent_Helper<PlayerInteraction>();
            if (!PlayerInventory) PlayerInventory = gameObject.GetComponent_Helper<PlayerInventory>();
            if (!PlayerGravity) PlayerGravity = gameObject.GetComponent_Helper<PlayerGravity>(); 
            if (!CameraPivot) CameraPivot = gameObject.FindObjectAndGetComponentInChildren_Helper<Transform>("CameraPivot");

            if (!FirstPersonCamera) FirstPersonCamera = GameObject.Find("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
            if (!ThirdPersonCamera) ThirdPersonCamera = GameObject.Find("ThirdPersonCamera").GetComponent<CinemachineVirtualCamera>();
            if (!DollyCart) DollyCart = GameObject.Find("Dolly Cart").GetComponent<CinemachineDollyCart>();
            if (!DollyTrack) DollyTrack = GameObject.Find("Dolly Track");
            
            AnimationData.Initialize();
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
            if (!CameraPivot) CameraPivot = gameObject.FindObjectAndGetComponentInChildren_Helper<Transform>("CameraPivot");

            if (!FirstPersonCamera) FirstPersonCamera = GameObject.Find("FirstPersonCamera")?.GetComponent<CinemachineVirtualCamera>();
            if (!ThirdPersonCamera) ThirdPersonCamera = GameObject.Find("ThirdPersonCamera")?.GetComponent<CinemachineVirtualCamera>();
            if (!DollyCart) DollyCart = GameObject.Find("Dolly Cart")?.GetComponent<CinemachineDollyCart>();
            if (!DollyTrack) DollyTrack = GameObject.Find("Dolly Track")?.gameObject;
            
            AnimationData.Initialize();
            
            nectTr = transform.FindChildByName<Transform>("DEF-neck");
            legLTr = transform.FindChildByName<Transform>("DEF-thigh.L");
            legRTr = transform.FindChildByName<Transform>("DEF-thigh.R");
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
            
            stateMachine.Player.SetScaleNeckLeg(0);
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

        private void OnDestroy()
        {
            if (deathCoroutine != null) StopCoroutine(deathCoroutine);
        }

        private void OnDeath()
        {
            PlayerInventory.OnDropWeapon(Vector3.zero);
            deathCoroutine = StartCoroutine(MoveDollyCart_Coroutine());
        }

        private IEnumerator MoveDollyCart_Coroutine()
        {
            SetScaleNeckLeg(1);
            
            Animator.SetLayerWeight(1, 0);
            FirstPersonCamera.Priority = 5; 
            ThirdPersonCamera.Priority = 10; 
            cam.cullingMask |= 1 << gameObject.layer;
            DollyTrack.transform.SetPositionAndRotation(new Vector3(transform.position.x, DollyTrack.transform.position.y, transform.position.z), transform.rotation);
            
            Animator.SetTrigger(AnimationData.DeathParameterHash);
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Death, 0.5f);
            
            DollyCart.m_Position = 0f;
            
            
            while (DollyCart.m_Position < 0.999f)
            {
                if (DollyCart.m_Position >= 0.5f)
                {
                    var speed = Mathf.Lerp(CartSpeed, 0f, Mathf.SmoothStep(0, 1, (DollyCart.m_Position - 0.5f) * 2f));
                    DollyCart.m_Position += Time.deltaTime * speed;
                } else DollyCart.m_Position += Time.deltaTime * CartSpeed;
                yield return null;
            }
            
            DollyCart.m_Position = 1f;
            deathCoroutine = null;
        }

        private void SetScaleNeckLeg(int endValue01)
        {
            Vector3 endVector = new Vector3(endValue01,endValue01,endValue01);
            nectTr.localScale = endVector;
            legLTr.localScale = endVector;
            legRTr.localScale = endVector;
        }

        public void SetCameraLayer(bool isVisible)
        {
            if(isVisible)
                cam.cullingMask |= 1 << gameObject.layer; 
            else
                cam.cullingMask &= ~(1 << gameObject.layer);
        }
    }
}
