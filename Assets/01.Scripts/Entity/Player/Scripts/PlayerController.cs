using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        // Properties of Player Input
        public InputActions PlayerInputs { get; private set; }
        public InputActions.PlayerActions PlayerActions { get; private set; }

        private void Awake()
        {
            PlayerInputs = new InputActions();
            PlayerActions = PlayerInputs.Player;
        }

        private void OnEnable()
        {
            PlayerInputs.Enable();
        }

        private void OnDisable()
        {
            PlayerInputs.Disable();
        }

        private void OnDestroy()
        {
            PlayerInputs.Dispose();
        }
    }
}
