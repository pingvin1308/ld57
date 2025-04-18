using UnityEngine;

namespace Game.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Canvas))]
    public class InventoryCanvasUI : MonoBehaviour
    {
        [field: SerializeField]
        public Canvas Canvas { get; private set; }

        [field: SerializeField]
        public ArtifactsUI ArtifactsUI { get; private set; }
        
        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }
    }
}