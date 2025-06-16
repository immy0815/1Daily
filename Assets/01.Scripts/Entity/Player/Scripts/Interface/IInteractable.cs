using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts.Interface
{
    public interface IInteractable
    {
        /// <summary>
        /// Used for Interactable Object (Not for Weapons!)
        /// </summary>
        void OnInteract();
        
        /// <summary>
        /// Used for Weapons
        /// </summary>
        /// <param name="pivot"></param>
        void OnInteract(Transform pivot, bool isOwnedByPlayer = false);
    }
}