using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Inventory
{
    public class ArtifactSlotUI : MonoBehaviour
    {
        private Image _slotImage;
        
        [field: SerializeField]
        public ArtifactItemUI Item { get; private set; }

        private void Awake()
        {
            _slotImage = GetComponent<Image>();
        }

        public void Set(ArtifactItemUI artifactGameObject)
        {
            Item = artifactGameObject;
        }

        public void Unset()
        {
            if (Item != null)
            {
                Destroy(Item.gameObject);
            }
        }
    }
}