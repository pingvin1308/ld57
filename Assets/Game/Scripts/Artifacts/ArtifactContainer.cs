using System.Collections;
using DG.Tweening;
using Game.Scripts.Artifacts;
using Game.Scripts.UI.Inventory;
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

        [SerializeField] private ArtifactPlaceholderArea artifactPlaceholderArea;
        [SerializeField] private InventoryCanvasUI _inventoryUI;
        [SerializeField] private Button _sellButton;
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
            StartCoroutine(MoveToContainer());
        }

        private void OnPlayerExited()
        {
            Debug.Log("ArtifactContainer: Player exited");
            _spriteRenderer.material.SetFloat("_Thickness", 0);
            StartCoroutine(MoveBack());
        }

        private void OnSellPressed()
        {
            if (Artifact == null) return;
            _orderProgress.CompleteOrder(new[] { Artifact });
            artifactPlaceholderArea.CleanUI();
            Artifact = null;
        }

        private IEnumerator MoveToContainer()
        {
            if (_isMoving) yield break;
            _isMoving = true;

            var artifactContainerOffset = new Vector3(0, 6f, 0);
            var worldUiScale = Vector3.one * 0.01f;
            var sortingOrder = 10;
            var canvasPlaneDistance = 5.0f;

            var screenPos = _inventoryUI.Canvas.transform.position;
            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, canvasPlaneDistance));

            _inventoryUI.Canvas.renderMode = RenderMode.WorldSpace;
            _inventoryUI.Canvas.worldCamera = Camera.main;
            _inventoryUI.transform.position = worldPos;
            _inventoryUI.Canvas.sortingOrder = sortingOrder;
            _inventoryUI.Canvas.GetComponent<RectTransform>().localScale = worldUiScale;
            var targetWorldPos = transform.position + artifactContainerOffset;
            yield return _inventoryUI.transform.DOMove(targetWorldPos, 0.5f).WaitForCompletion();

            _isMoving = false;
        }

        private IEnumerator MoveBack()
        {
            if (_isMoving) yield break;
            _isMoving = true;
            _inventoryUI.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            yield return null;
            _isMoving = false;
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