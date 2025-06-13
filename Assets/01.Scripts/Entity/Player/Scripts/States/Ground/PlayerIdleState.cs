using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts.States.Ground
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(PlayerStateMachine machine) : base(machine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
        }
        
        public override void Update()
        {
            base.Update();

            if (stateMachine.MovementDirection == Vector2.zero) return;
            if (stateMachine.Player.CharacterController.velocity.magnitude >= playerCondition.WalkSpeedThreshold)
                stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
}