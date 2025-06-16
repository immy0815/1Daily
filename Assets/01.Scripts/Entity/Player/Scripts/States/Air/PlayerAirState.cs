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
            //Debug.Log("Changed Time scale to 0.3f");

            base.OnSlowMotionPerformed(context);
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Jump, 0.3f);
        }

        protected override void OnSlowMotionCanceled(InputAction.CallbackContext context)
        {
            //Debug.Log("Changed Time scale to 1f");

            base.OnSlowMotionCanceled(context);
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Jump, 1f);
        }

        protected override void OnAttack(InputAction.CallbackContext context)
        {
            base.OnAttack(context);
            if (stateMachine.Player.PlayerInventory.CurrentWeapon is Pistol pistol)
            {
                if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
                AttackCoroutine = StartCoroutine(ChangeTimeScaleForSeconds(0.5f));
                if (pistol.OnShoot())
                {
                    // TODO: Animation 호출
                }
                else
                {
                    // TODO: 탄환 없음 UI 출력
                }
                return;
            }

            if (stateMachine.Player.PlayerInteraction.Interactable is not Enemy enemy) return;
            if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
            AttackCoroutine = StartCoroutine(ChangeTimeScaleForSeconds(0.5f));
            if (stateMachine.Player.PlayerInventory.CurrentWeapon is Katana katana)
            {
                // TODO: Animation 호출
                katana.OnHit();
                enemy.OnTakeDamage(katana.WeaponData.damage);
            }
            else
            {
                // TODO: Animation 호출
                enemy.OnTakeDamage(stateMachine.Player.PlayerCondition.Damage);
            }
            stateMachine.Player.PlayerInteraction.ResetParameters();
        }

        protected override void OnPickOrThrow(InputAction.CallbackContext context)
        {
            base.OnPickOrThrow(context);

            if (stateMachine.Player.PlayerInventory.CurrentWeapon)
            {
                // TODO: Animation 호출?
                stateMachine.Player.PlayerInventory.OnDropWeapon(stateMachine.Player.MainCameraTransform.forward);
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