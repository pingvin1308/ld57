using UnityEngine;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class OxygenCanvasUI : MonoBehaviour
    {
        [field: SerializeField]
        public Canvas Canvas { get; private set; }

        [field: SerializeField]
        public OxygenUI OxygenUI { get; private set; }

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }
    }
}