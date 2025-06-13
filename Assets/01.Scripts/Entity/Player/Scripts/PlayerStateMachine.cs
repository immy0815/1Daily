using System;
using _01.Scripts.Entity.Common.Scripts;
using _01.Scripts.Entity.Player.Scripts.States.Air;
using _01.Scripts.Entity.Player.Scripts.States.Ground;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    [Serializable] public class PlayerStateMachine : StateMachine
    {
        // Player States
        public PlayerIdleState IdleState { get; private set; }
        public PlayerWalkState WalkState { get; private set; }
        public PlayerRunState RunState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerFallState FallState { get; private set; }
        
        // Properties
        public Player Player { get; private set; }
        public Vector2 MovementDirection { get; set; }
        public float MovementSpeed { get; private set; }
        public float RotationalDamping { get; private set; }
        public float JumpForce { get; set; }
        public Transform MainCameraTransform { get; set; }
        public int ComboIndex { get; set; }

        // Constructor
        public PlayerStateMachine(Player player)
        {
            Player = player;
            JumpForce = player.PlayerCondition.JumpForce;
            MainCameraTransform = player.MainCameraTransform;
            MovementSpeed = player.PlayerCondition.Speed;
            RotationalDamping = player.PlayerCondition.RotationalDamping;
            
            // Registration of States
            IdleState = new PlayerIdleState(this);
            WalkState = new PlayerWalkState(this);
            RunState = new PlayerRunState(this);
            JumpState = new PlayerJumpState(this);
            FallState = new PlayerFallState(this);
        }
    }
}