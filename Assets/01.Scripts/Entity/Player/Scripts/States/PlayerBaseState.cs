using _01.Scripts.Entity.Common.Scripts;
using _01.Scripts.Entity.Common.Scripts.Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Scripts.Entity.Player.Scripts.States
{
    public class PlayerBaseState : MonoBehaviour, IState
    {
        protected readonly PlayerStateMachine stateMachine;
        protected readonly EntityCondition playerCondition;

        public PlayerBaseState(PlayerStateMachine machine)
        {
            stateMachine = machine;
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
            Move();
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
            Rotate(movementDirection);
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
            if (direction == Vector3.zero) return;
            
            var unitTransform = stateMachine.Player.transform;
            var targetRotation = Quaternion.LookRotation(direction);
            unitTransform.rotation = Quaternion.Slerp(unitTransform.rotation, targetRotation, stateMachine.RotationalDamping * Time.deltaTime);
        }

        protected virtual void AddInputActionCallbacks()
        {
            var unitController = stateMachine.Player.PlayerController;
            unitController.PlayerActions.Move.canceled += OnMoveCanceled;
            unitController.PlayerActions.Jump.started += OnJumpStarted;
        }
        
        protected virtual void RemoveInputActionCallbacks()
        {
            var unitController = stateMachine.Player.PlayerController;
            unitController.PlayerActions.Move.canceled -= OnMoveCanceled;
            unitController.PlayerActions.Jump.started -= OnJumpStarted;
        }

        protected virtual void OnMoveCanceled(InputAction.CallbackContext context) { }
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (playerCondition.IsDead) return;
        }
        protected virtual void OnSlowMotion(InputAction.CallbackContext context)
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