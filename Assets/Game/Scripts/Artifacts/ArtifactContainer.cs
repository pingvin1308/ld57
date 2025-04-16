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

        private ArtifactData _artifact;

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
            if (_artifact == null) return;
            _orderProgress.CompleteOrder(new[] { _artifact });
            artifactPlaceholderArea.CleanUI();
            _artifact = null;
        }

        private void OnArtifactPlaced(ArtifactData artifactData)
        {
            if (_artifact != null)
            {
                Debug.Log("ArtifactContainer: Not enough place");
                artifactPlaceholderArea.DropArtifact(artifactData);
                return;
            }

            Debug.Log("ArtifactContainer: Artifact placed");
            _artifact = artifactData;
            artifactPlaceholderArea.UpdateUI(artifactData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_artifact == null || artifactPlaceholderArea.Player == null) return;

            Debug.Log("ArtifactContainer: Artifact collected");
            artifactPlaceholderArea.DropArtifact(_artifact);
            artifactPlaceholderArea.CleanUI();
            _artifact = null;
        }
    }
}