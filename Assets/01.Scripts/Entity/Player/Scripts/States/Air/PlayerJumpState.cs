using _01.Scripts.Manager;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts.States.Air
{
    public class PlayerJumpState : PlayerAirState
    {
        public PlayerJumpState(PlayerStateMachine machine) : base(machine)
        {
        }
        
        public override void Enter()
        {
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Jump, 1f);
            stateMachine.JumpForce = stateMachine.Player.PlayerCondition.JumpForce;
            stateMachine.Player.PlayerGravity.Jump(stateMachine.JumpForce);
            
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        }

        public override void Update()
        {
            base.Update();
            if(stateMachine.Player.CharacterController.velocity.y <= 0) 
                stateMachine.ChangeState(stateMachine.FallState);
        }
    }
}