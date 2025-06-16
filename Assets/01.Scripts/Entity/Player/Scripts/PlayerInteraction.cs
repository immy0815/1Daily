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
        [SerializeField] private GameObject detectedObject;

        // Fields
        [Header("Last Check Time")]
        [SerializeField, ReadOnly] private float timeSinceLastCheck;
        private Camera cam;
        private Player player;
        
        // Properties
        public IInteractable Interactable { get; private set; }
        public IDamagable Damagable { get; private set; }
        
        // Start is called before the first frame update
        private void Start()
        {
            cam = Camera.main;
            timeSinceLastCheck = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            if (timeSinceLastCheck < checkRate) { timeSinceLastCheck += Time.unscaledDeltaTime; return; }

            timeSinceLastCheck = 0;
            var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out var hit, maxCheckDistance, interactableLayers))
            {
                if (hit.collider.gameObject == detectedObject) return;
                
                detectedObject = hit.collider.gameObject;
                if (detectedObject.TryGetComponent<IInteractable>(out var interactable)){ Interactable = interactable; Damagable = null; }
                else if(detectedObject.TryGetComponent<IDamagable>(out var damagable)) { Interactable = null; Damagable = damagable; }
                else { Interactable = null; Damagable = null; }
            }
            else
            {
                detectedObject = null;
                Interactable = null;
                Damagable = null;
            }
        }

        public void Init(Player player)
        {
            this.player = player;
        }

        public void OnInteract()
        {
            Interactable.OnInteract();
            ResetParameters();
        }

        public void OnDamage(int damage)
        {
            Damagable.OnTakeDamage(damage);
            ResetParameters();
        }

        public void ResetParameters()
        {
            detectedObject = null;
            Interactable = null;
            Damagable = null;
        }
    }
}
