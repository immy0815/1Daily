using _01.Scripts.Util;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    public class PlayerGravity : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        
        private float verticalVelocity;
        
        public Vector3 ExtraMovement => Vector3.up * verticalVelocity;

        private void Awake()
        {
            if(!characterController) characterController = gameObject.GetComponent_Helper<CharacterController>();
        }

        private void Reset()
        {
            if(!characterController) characterController = gameObject.GetComponent_Helper<CharacterController>();
        }

        private void Update()
        {
            if (characterController.isGrounded && verticalVelocity < 0f) verticalVelocity = Physics.gravity.y * Time.deltaTime;
            else verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        
        public void Jump(float jumpForce)
        {
            verticalVelocity = jumpForce;
        }
    }
}