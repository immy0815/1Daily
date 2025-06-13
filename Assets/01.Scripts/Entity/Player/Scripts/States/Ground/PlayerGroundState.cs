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
            if (stateMachine.MovementDirection == Vector2.zero) {} // TODO: Change Time scale to 0.01 or so
            else {} // TODO: Change Time scale to 1
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
    }
}