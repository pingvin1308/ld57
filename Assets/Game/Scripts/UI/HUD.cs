using Game.Scripts.UI.Inventory;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class HUD : MonoBehaviour
    {
        [field: SerializeField]
        public OxygenCanvasUI OxygenCanvasUI { get; private set; }

        [field: SerializeField]
        public DetectorCanvasUI DetectorCanvasUI { get; private set; }

        [field: SerializeField]
        public InventoryCanvasUI InventoryCanvasUI { get; private set; }

        private void Start()
        {
            OxygenCanvasUI.gameObject.SetActive(false);
            DetectorCanvasUI.gameObject.SetActive(false);
            InventoryCanvasUI.gameObject.SetActive(false);
        }
    }
}