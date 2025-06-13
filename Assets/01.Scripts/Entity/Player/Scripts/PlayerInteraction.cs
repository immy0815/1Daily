using _01.Scripts.Entity.Player.Scripts.Interface;
using UnityEngine;

namespace _01.Scripts.Entity.Player.Scripts
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float checkRate = 0.05f;
        [SerializeField] private float maxCheckDistance = 5f;
        [SerializeField] private LayerMask interactableLayers;
        [SerializeField] private GameObject interactableObject;

        // Fields
        private float timeSinceLastCheck;
        private Camera camera;
        
        // Properties
        public IInteractable Interactable { get; private set; }
        
        // Start is called before the first frame update
        private void Start()
        {
            camera = Camera.main;
            timeSinceLastCheck = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            if (timeSinceLastCheck < checkRate) { timeSinceLastCheck += Time.unscaledDeltaTime; return; }

            timeSinceLastCheck = 0;
            var ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out var hit, maxCheckDistance, interactableLayers))
            {
                if (hit.collider.gameObject == interactableObject) return;
                
                interactableObject = hit.collider.gameObject;
                if (!interactableObject.TryGetComponent<IInteractable>(out var interactable)) return;
                Interactable = interactable;
            }
            else
            {
                interactableObject = null;
                Interactable = null;
            }
        }

        public void OnInteract()
        {
            Interactable.OnInteract();
            interactableObject = null;
            Interactable = null;
        }
    }
}
