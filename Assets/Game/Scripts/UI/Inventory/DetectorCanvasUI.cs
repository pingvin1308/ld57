using UnityEngine;

namespace Game.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Canvas))]
    public class DetectorCanvasUI : MonoBehaviour
    {
        [field: SerializeField]
        public Canvas Canvas { get; private set; }

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }
    }
}