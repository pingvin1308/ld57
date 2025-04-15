using Game.Scripts.UI;
using Game.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DetectorUpgrader : MonoBehaviour
    {
        private bool _isMoving;
        [SerializeField] private InteractionArea _interactionArea;
        [SerializeField] private DetectorCanvasUI _detectorCanvasUI;
        [SerializeField] private UIWorldFollower _detectorUI;
        [SerializeField] private Button _buyUpgradeButton;
        [SerializeField] private Vector3 _worldOffest;

        private SpriteRenderer _spriteRenderer;

        public (int Price, int Value)[] DetectorUpgrades => new[]
        {
            (Price: 0, Value: 0),
            (Price: 10, Value: 1),
            (Price: 100, Value: 2),
            (Price: 300, Value: 3),
        };

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _buyUpgradeButton.gameObject.SetActive(false);
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
            if (DetectorUpgrades.Length == upgradeLevel) return;

            var upgrade = DetectorUpgrades[upgradeLevel];

            if (player.Inventory.SpendMoney(upgrade.Price))
            {
                player.Oxygen.ApplyUpgrade(upgrade.Value, upgradeLevel);
                // _inventoryCanvasUI..SetUpgradeLevel(upgradeLevel);
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
            _detectorUI.StartFollowing(transform.position + _worldOffest);
            _buyUpgradeButton.gameObject.SetActive(true);
        }

        private void OnPlayerExited()
        {
            Debug.Log("ArtifactContainer: Player exited");
            _spriteRenderer.material.SetFloat("_Thickness", 0);
            _detectorUI.StopFollowing();
            _buyUpgradeButton.gameObject.SetActive(false);
        }
    }
}