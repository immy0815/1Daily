namespace _01.Scripts.Entity.Player.Scripts.States.Ground
{
    public class PlayerWalkState : PlayerGroundState
    {
        public PlayerWalkState(PlayerStateMachine machine) : base(machine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
        }
        
        public override void Update()
        {
            base.Update();

            if (stateMachine.Player.CharacterController.velocity.magnitude < playerCondition.WalkSpeedThreshold)
                stateMachine.ChangeState(stateMachine.IdleState);
            else if(stateMachine.Player.CharacterController.velocity.magnitude > playerCondition.WalkSpeedThreshold)
                stateMachine.ChangeState(stateMachine.RunState);
        }
    }
}