using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts.States.Ground
{
    public class PlayerRunState : PlayerGroundState
    {
        public PlayerRunState(PlayerStateMachine machine) : base(machine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
        }
        
        public override void Update()
        {
            base.Update();

            if (stateMachine.Player.CharacterController.velocity.magnitude < playerCondition.RunSpeedThreshold)
                stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
}