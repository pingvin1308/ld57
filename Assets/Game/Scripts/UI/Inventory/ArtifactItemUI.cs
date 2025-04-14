using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Image))]
    public class ArtifactItemUI : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent<ArtifactData, int> ArtifactDroped;
        
        [field: SerializeField]
        public int Index { get; private set; }
        
        [field: SerializeField]
        public Image Image { get; private set; }

        [field: SerializeField]
        public ArtifactData Artifact { get; private set; }
        
        private void Awake()
        {
            Image = GetComponent<Image>();
        }

        public void Init(ArtifactData artifact, int index)
        {
            Artifact = artifact;
            Image.sprite = artifact.Sprite;
            Index = index;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ArtifactDroped?.Invoke(Artifact, Index);
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            ArtifactDroped?.RemoveAllListeners();
        }
    }
}