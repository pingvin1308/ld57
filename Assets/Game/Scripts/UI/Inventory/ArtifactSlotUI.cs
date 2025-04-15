using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Image))]
    public class ArtifactSlotUI : MonoBehaviour
    {
        [SerializeField] private Sprite _closedSlot;
        [SerializeField] private Sprite _openedSlot;
        [SerializeField] private Image _slotImage;

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

        public void Unlock()
        {
            _slotImage.sprite = _openedSlot;
        }

        public void Lock()
        {
            _slotImage.sprite = _closedSlot;
        }
    }
}