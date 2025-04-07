using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Image))]
    public class ArtifactItemUI : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent<ArtifactData> ArtifactDroped;
        
        [field: SerializeField]
        public Image Image { get; private set; }

        public Guid Id { get; } = Guid.NewGuid();

        [field: SerializeField]
        public ArtifactData Artifact { get; private set; }
        
        private void Awake()
        {
            Image = GetComponent<Image>();
        }

        public void Init(ArtifactData artifact)
        {
            Artifact = artifact;
            Image.sprite = artifact.Sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Item clicked {Id}");
            ArtifactDroped?.Invoke(Artifact);
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            ArtifactDroped?.RemoveAllListeners();
        }
    }
}