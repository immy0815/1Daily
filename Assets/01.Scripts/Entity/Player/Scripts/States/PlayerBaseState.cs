using System.Collections;
using _01.Scripts.Entity.Common.Scripts;
using _01.Scripts.Entity.Common.Scripts.Interface;
using _01.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Scripts.Entity.Player.Scripts.States
{
    public class PlayerBaseState : IState
    {
        protected readonly PlayerStateMachine stateMachine;
        protected readonly EntityCondition playerCondition;
        protected Coroutine AttackCoroutine;
        protected Coroutine normalAttackCoroutine;
        private int comboIndex = -1;

        public PlayerBaseState(PlayerStateMachine machine)
        {
            stateMachine = machine;
            AttackCoroutine = null;
            playerCondition = stateMachine.Player.PlayerCondition;
        }
        
        public virtual void Enter()
        {
            AddInputActionCallbacks();
        }

        public virtual void HandleInput()
        {
            if (playerCondition.IsDead) { stateMachine.MovementDirection = Vector2.zero; return; }
            ReadMovementInput();
        }

        public virtual void Update()
        {
            if (playerCondition.IsDead) { return; }
            Move();
            Rotate(stateMachine.Player.MainCameraTransform.forward);
        }

        public virtual void LateUpdate()
        {
            
        }

        public virtual void PhysicsUpdate()
        {
            
        }

        public virtual void Exit()
        {
            RemoveInputActionCallbacks();
        }
        
        protected void StartAnimation(int animatorHash)
        {
            stateMachine.Player.Animator.SetBool(animatorHash, true);
        }

        protected void StopAnimation(int animatorHash)
        {
            stateMachine.Player.Animator.SetBool(animatorHash, false);
        }
        
        protected virtual void ReadMovementInput()
        {
            stateMachine.MovementDirection =
                stateMachine.Player.PlayerController.PlayerActions.Move.ReadValue<Vector2>();
        }
        
        private void Move()
        {
            var movementDirection = GetMovementDirection();
            Move(movementDirection);
        }

        private Vector3 GetMovementDirection()
        {
            var forward = stateMachine.MainCameraTransform.forward;
            var right = stateMachine.MainCameraTransform.right;


            forward.y = 0;
            right.y = 0;
            
            forward.Normalize();
            right.Normalize();
            
            return forward * stateMachine.MovementDirection.y + right * stateMachine.MovementDirection.x;
        }

        private void Move(Vector3 direction)
        {
            var movementSpeed = GetMovementSpeed();
            stateMachine.Player.CharacterController.Move((direction * movementSpeed + stateMachine.Player.PlayerGravity.ExtraMovement) * Time.deltaTime);
        }
        
        private float GetMovementSpeed()
        {
            var movementSpeed = stateMachine.MovementSpeed;
            return movementSpeed;
        }

        private void Rotate(Vector3 direction)
        {
            if (playerCondition.IsDead) return;
            if (direction == Vector3.zero) return;
            
            var unitTransform = stateMachine.Player.transform;
            var cameraPivotTransform = stateMachine.Player.PlayerInventory.CameraPivot; 
            
            var unitDirection = new Vector3(direction.x, 0, direction.z);
            var targetRotation = Quaternion.LookRotation(unitDirection);
            unitTransform.rotation = Quaternion.Slerp(unitTransform.rotation, targetRotation, stateMachine.RotationalDamping * Time.unscaledDeltaTime);
            
            var cameraTargetRotation = Quaternion.LookRotation(direction);
            cameraPivotTransform.rotation = cameraTargetRotation;
        }
        
        protected IEnumerator ChangeTimeScaleForSeconds(float timeDuration)
        {
            var time = 0f;
            var startTimeScale = TimeScaleManager.Instance.MaxTimeScale;
            var targetTimeScale = TimeScaleManager.Instance.MinTimeScale;
            
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Attack, startTimeScale);
            while (time < timeDuration)
            {
                time += Time.unscaledDeltaTime;
                var t = time / timeDuration;
                var lerpScale = Mathf.Lerp(startTimeScale, targetTimeScale, t);
                TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Attack, lerpScale);
                yield return null;
            }
            
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Attack, targetTimeScale);
            AttackCoroutine = null;
        }
        
        protected IEnumerator PlayFistAttackAnimation()
        {
            stateMachine.Player.SetCameraLayer(true);
            
            StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
            stateMachine.Player.Animator.SetInteger("NormalCombo", comboIndex = comboIndex++ % 3);
            yield return new WaitForSecondsRealtime(1f);
            StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
            
            stateMachine.Player.SetCameraLayer(false);
        }

        private void AddInputActionCallbacks()
        {
            var playerController = stateMachine.Player.PlayerController;
            playerController.PlayerActions.Move.canceled += OnMoveCanceled;
            playerController.PlayerActions.Jump.started += OnJumpStarted;
            playerController.PlayerActions.SlowMotion.performed += OnSlowMotionPerformed;
            playerController.PlayerActions.SlowMotion.canceled += OnSlowMotionCanceled;
            playerController.PlayerActions.Attack.started += OnAttack;
            playerController.PlayerActions.PickOrThrow.started += OnPickOrThrow;
        }
        
        private void RemoveInputActionCallbacks()
        {
            var playerController = stateMachine.Player.PlayerController;
            playerController.PlayerActions.Move.canceled -= OnMoveCanceled;
            playerController.PlayerActions.Jump.started -= OnJumpStarted;
            playerController.PlayerActions.SlowMotion.performed -= OnSlowMotionPerformed;
            playerController.PlayerActions.SlowMotion.canceled -= OnSlowMotionCanceled;
            playerController.PlayerActions.Attack.started -= OnAttack;
            playerController.PlayerActions.PickOrThrow.started -= OnPickOrThrow;
        }

        protected virtual void OnMoveCanceled(InputAction.CallbackContext context) { }
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (playerCondition.IsDead) return;
        }
        protected virtual void OnSlowMotionPerformed(InputAction.CallbackContext context)
        {
            if (playerCondition.IsDead) return;
        }

        protected virtual void OnSlowMotionCanceled(InputAction.CallbackContext context)
        {
            if (playerCondition.IsDead) return;
        }
        
        protected virtual void OnAttack(InputAction.CallbackContext context)
        {
            if (playerCondition.IsDead) return;
        }
        protected virtual void OnPickOrThrow(InputAction.CallbackContext context)
        {
            if (playerCondition.IsDead) return;
        }
    }
}