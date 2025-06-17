using _01.Scripts.Manager;

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
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Jump, 0.01f);
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