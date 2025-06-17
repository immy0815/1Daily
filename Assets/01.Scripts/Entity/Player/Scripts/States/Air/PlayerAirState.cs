using _01.Scripts.Manager;
using UnityEngine;
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
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.AirParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.AirParameterHash);
        }

        protected override void OnSlowMotionPerformed(InputAction.CallbackContext context)
        {
            // Debug.Log("Changed Time scale to 0.3f");

            base.OnSlowMotionPerformed(context);
            if (playerCondition.IsDead) return;
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Jump, 0.3f);
        }

        protected override void OnSlowMotionCanceled(InputAction.CallbackContext context)
        {
            // Debug.Log("Changed Time scale to 1f");

            base.OnSlowMotionCanceled(context);
            if (playerCondition.IsDead) return;
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Jump, 1f);
        }

        protected override void OnAttack(InputAction.CallbackContext context)
        {
            base.OnAttack(context);
            if (playerCondition.IsDead) return;
            if (stateMachine.Player.PlayerInventory.CurrentWeapon is Pistol pistol)
            {
                if (!pistol.OnShoot(stateMachine.Player)) return;
                if (AttackCoroutine != null) stateMachine.Player.StopCoroutine(AttackCoroutine); 
                // TODO: Animation 호출
                return;
            }

            if (stateMachine.Player.PlayerInteraction.Damagable is not Enemy) return;
            if (AttackCoroutine != null) stateMachine.Player.StopCoroutine(AttackCoroutine);
            if (stateMachine.Player.PlayerInventory.CurrentWeapon is Katana katana)
            {
                // TODO: Animation 호출
                katana.OnHit();
                Debug.Log("Katana Attack");
                stateMachine.Player.PlayerInteraction.Damagable.OnTakeDamage(katana.WeaponData.damage);
            }
            else
            {
                // TODO: Animation 호출
                Debug.Log("Fist Attack");
                stateMachine.Player.PlayerInteraction.Damagable.OnTakeDamage(stateMachine.Player.PlayerCondition.Damage);
            }
            stateMachine.Player.PlayerInteraction.ResetParameters();
        }

        protected override void OnPickOrThrow(InputAction.CallbackContext context)
        {
            base.OnPickOrThrow(context);
            if (playerCondition.IsDead) return;
            if (stateMachine.Player.PlayerInventory.CurrentWeapon)
            {
                // TODO: Animation 호출?
                stateMachine.Player.PlayerInventory.OnDropWeapon(Physics.Raycast(
                    stateMachine.Player.MainCameraTransform.position,
                    stateMachine.Player.MainCameraTransform.forward, out var hitInfo, float.MaxValue)
                    ? (hitInfo.point - stateMachine.Player.PlayerInventory.WeaponPivot.position).normalized
                    : stateMachine.Player.MainCameraTransform.forward);
                return;
            }

            switch (stateMachine.Player.PlayerInteraction.Interactable)
            {
                case null: return;
                case Weapon weapon:
                    stateMachine.Player.PlayerInventory.OnEquipWeapon(weapon); 
                    stateMachine.Player.PlayerInteraction.ResetParameters();
                    break;
                default:
                    stateMachine.Player.PlayerInteraction.OnInteract(); break;
            }
        }
    }
}