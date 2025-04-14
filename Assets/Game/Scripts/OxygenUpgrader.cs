using System.Collections;
using DG.Tweening;
using Game.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class OxygenUpgrader : MonoBehaviour
    {
        private bool _isMoving;
        [SerializeField] private InteractionArea _interactionArea;
        [SerializeField] private OxygenCanvasUI _oxygenCanvasUI;
        [SerializeField] private Button _buyUpgradeButton;
        private SpriteRenderer _spriteRenderer;

        public (int Price, int Value)[] OxygenUpgrades => new[]
        {
            (Price: 0, Value: 0),
            (Price: 100, Value: 300),
            (Price: 500, Value: 1500),
            (Price: 1500, Value: 6300)
        };
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _interactionArea.PlayerEntered.AddListener(OnPlayerEntered);
            _interactionArea.PlayerExited.AddListener(OnPlayerExited);
            _buyUpgradeButton.onClick.AddListener(OnBuyPressed);
        }

        private void OnBuyPressed()
        {
            if (_interactionArea.Player == null) return;
            var player = _interactionArea.Player;

            var upgradeLevel = player.Oxygen.UpgradeLevel + 1;
            if (OxygenUpgrades.Length == upgradeLevel) return;

            var upgrade = OxygenUpgrades[upgradeLevel];

            if (player.Inventory.SpendMoney(upgrade.Price))
            {
                player.Oxygen.ApplyUpgrade(upgrade.Value, upgradeLevel);
                _oxygenCanvasUI.OxygenUI.BalloonImage.SetUpgradeLevel(upgradeLevel);
                Debug.Log("Апгрейд куплен!");
            }
            else
            {
                Debug.Log("Не хватает денех!");
            }
        }

        private void OnDisable()
        {
            _interactionArea.PlayerEntered.RemoveListener(OnPlayerEntered);
            _interactionArea.PlayerExited.RemoveListener(OnPlayerExited);
            _buyUpgradeButton.onClick.RemoveListener(OnBuyPressed);
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

        private IEnumerator MoveToContainer()
        {
            var artifactContainerOffset = new Vector3(-1.8f, 3.3f, 0);
            var worldUiScale = Vector3.one * 0.01f;
            var sortingOrder = 10;
            var canvasPlaneDistance = 5.0f;

            var screenPos = _oxygenCanvasUI.Canvas.transform.position;
            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, canvasPlaneDistance));

            _oxygenCanvasUI.Canvas.transform.position = worldPos;
            _oxygenCanvasUI.Canvas.renderMode = RenderMode.WorldSpace;
            _oxygenCanvasUI.Canvas.worldCamera = Camera.main;
            _oxygenCanvasUI.Canvas.sortingOrder = sortingOrder;
            _oxygenCanvasUI.Canvas.GetComponent<RectTransform>().localScale = worldUiScale;

            var targetWorldPos = transform.position + artifactContainerOffset;
            yield return _oxygenCanvasUI.OxygenUI.transform.DOMove(targetWorldPos, 0.5f).WaitForCompletion();
        }

        private IEnumerator MoveBack()
        {
            _oxygenCanvasUI.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _oxygenCanvasUI.OxygenUI.ResetPosition();
            yield return null;
        }
    }
}