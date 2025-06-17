using _01.Scripts.Manager;
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
            if (stateMachine.MovementDirection != Vector2.zero)
            {
                if (AttackCoroutine != null){ stateMachine.Player.StopCoroutine(AttackCoroutine); AttackCoroutine = null; }
                stateMachine.Player.PlayerInventory.ResetThrowCoroutine();
                if(!Mathf.Approximately(Time.timeScale, 1))
                    TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Move, 1f);
                return;
            }

            if (Mathf.Approximately(Time.timeScale, 0.01f)) return;
            if (AttackCoroutine != null) return;
            if (stateMachine.Player.PlayerInventory.ThrowCoroutine != null) return;
                
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Move, 0.01f);
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
            if (playerCondition.IsDead) return;
            stateMachine.ChangeState(stateMachine.JumpState);
        }

        protected override void OnAttack(InputAction.CallbackContext context)
        {
            base.OnAttack(context);
            if (playerCondition.IsDead) return;
            if (stateMachine.Player.PlayerInventory.CurrentWeapon is Pistol pistol)
            {
                if (!pistol.OnShoot(stateMachine.Player)) return;
                if (AttackCoroutine != null) stateMachine.Player.StopCoroutine(AttackCoroutine); 
                AttackCoroutine = stateMachine.Player.StartCoroutine(ChangeTimeScaleForSeconds(0.5f));
                // TODO: Animation 호출
                return;
            }

            if (stateMachine.Player.PlayerInteraction.Damagable is not Enemy) return;
            if (AttackCoroutine != null) stateMachine.Player.StopCoroutine(AttackCoroutine);
            AttackCoroutine = stateMachine.Player.StartCoroutine(ChangeTimeScaleForSeconds(1f));
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