using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts.States.Air
{
    public class PlayerFallState : PlayerAirState
    {
        public PlayerFallState(PlayerStateMachine machine) : base(machine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
        }

        public override void Update()
        {
            base.Update();
            if(stateMachine.Player.CharacterController.isGrounded) 
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}