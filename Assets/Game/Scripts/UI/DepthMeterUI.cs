using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class DepthMeterUI : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI Value { get; private set; }
        
        [field: SerializeField]
        public Player Player { get; private set; }

        private void OnEnable()
        {
            Player.LevelEntered.AddListener(OnLevelEntered);
        }

        private void OnDisable()
        {
            Player.LevelEntered.RemoveListener(OnLevelEntered);
        }

        private void OnLevelEntered()
        {
            Value.text = Player.CurrentLevel.ToString();
        }
    }
}