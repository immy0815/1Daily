using UnityEngine.InputSystem;

namespace _01.Scripts.Entity.Player.Scripts.States.Air
{
    public class PlayerAirState : PlayerBaseState
    {
        public PlayerAirState(PlayerStateMachine machine) : base(machine)
        {
        }
        
        public override void Enter()
        {
            // TODO: Change Time scale to 1
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.AirParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.AirParameterHash);
        }

        protected override void OnSlowMotion(InputAction.CallbackContext context)
        {
            base.OnSlowMotion(context);
            if (context.performed)
            {
                //TODO: Change Time Scale to smaller value
            }
        }
    }
}