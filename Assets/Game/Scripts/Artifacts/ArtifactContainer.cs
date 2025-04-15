using Game.Scripts.Artifacts;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ArtifactContainer : MonoBehaviour, IPointerClickHandler
    {
        private bool _isMoving;
        private SpriteRenderer _spriteRenderer;
        
        [Header("UI")]
        [SerializeField] private UIWorldFollower _inventoryUI;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Vector3 _worldOffest;
        
        [Header("State")]
        [SerializeField] private ArtifactPlaceholderArea artifactPlaceholderArea;
        [SerializeField] private OrderProgress _orderProgress;
        
        [field: SerializeField]
        public ArtifactData Artifact { get; private set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            artifactPlaceholderArea.PlayerEntered.AddListener(OnPlayerEntered);
            artifactPlaceholderArea.PlayerExited.AddListener(OnPlayerExited);
            artifactPlaceholderArea.ArtifactPlaced.AddListener(OnArtifactPlaced);
            _sellButton.onClick.AddListener(OnSellPressed);
        }

        private void OnDisable()
        {
            artifactPlaceholderArea.PlayerEntered.RemoveListener(OnPlayerEntered);
            artifactPlaceholderArea.PlayerExited.RemoveListener(OnPlayerExited);
            artifactPlaceholderArea.ArtifactPlaced.RemoveListener(OnArtifactPlaced);
            _sellButton.onClick.RemoveListener(OnSellPressed);
        }

        private void OnPlayerEntered()
        {
            Debug.Log("ArtifactContainer: Player entered");
            _spriteRenderer.material.SetFloat("_Thickness", 1.0f);
            _inventoryUI.StartFollowing(transform.position + _worldOffest);
        }

        private void OnPlayerExited()
        {
            Debug.Log("ArtifactContainer: Player exited");
            _spriteRenderer.material.SetFloat("_Thickness", 0);
            _inventoryUI.StopFollowing();
        }

        private void OnSellPressed()
        {
            if (Artifact == null) return;
            _orderProgress.CompleteOrder(new[] { Artifact });
            artifactPlaceholderArea.CleanUI();
            Artifact = null;
        }

        private void OnArtifactPlaced(ArtifactData artifactData)
        {
            if (Artifact != null)
            {
                Debug.Log("ArtifactContainer: Not enough place");
                artifactPlaceholderArea.DropArtifact(artifactData);
                return;
            }

            Debug.Log("ArtifactContainer: Artifact placed");
            Artifact = artifactData;
            artifactPlaceholderArea.UpdateUI(artifactData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Artifact == null || artifactPlaceholderArea.Player == null) return;

            Debug.Log("ArtifactContainer: Artifact collected");
            artifactPlaceholderArea.DropArtifact(Artifact);
            artifactPlaceholderArea.CleanUI();
            Artifact = null;
        }
    }
}