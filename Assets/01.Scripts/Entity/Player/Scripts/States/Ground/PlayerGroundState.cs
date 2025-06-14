using _01.Scripts.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Scripts.Entity.Player.Scripts.States.Ground
{
    public class PlayerGroundState : PlayerBaseState
    {
        public PlayerGroundState(PlayerStateMachine machine) : base(machine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!stateMachine.Player.CharacterController.isGrounded &&
                stateMachine.Player.CharacterController.velocity.y < Physics.gravity.y * Time.deltaTime)
            {
                stateMachine.ChangeState(stateMachine.FallState);
            }
        }

        protected override void ReadMovementInput()
        {
            base.ReadMovementInput();
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Move, stateMachine.MovementDirection == Vector2.zero ? 0.01f : 1f);
        }

        protected override void OnMoveCanceled(InputAction.CallbackContext context)
        {
            if (stateMachine.MovementDirection == Vector2.zero) return;
            
            base.OnMoveCanceled(context);
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            base.OnJumpStarted(context);
            stateMachine.ChangeState(stateMachine.JumpState);
        }

        protected override void OnAttack(InputAction.CallbackContext context)
        {
            base.OnAttack(context);
            if (stateMachine.Player.PlayerInventory.CurrentWeapon is Pistol pistol)
            {
                
                return;
            }

            if (stateMachine.Player.PlayerInteraction.Interactable is not Enemy enemy) return;
            if (stateMachine.Player.PlayerInventory.CurrentWeapon is Katana katana)
            {
                //TODO: Animation 호출, Enemy 데미지 호출 함수
            }
            else
            {
                //TODO: Animation 호출, Enemy 데미지 호출 함수
            }
        }
    }
}