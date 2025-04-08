using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class OrderProgressUI : MonoBehaviour
    {
        [field: SerializeField]
        public Slider Progress { get; private set; }
        
        [field: SerializeField]
        public TextMeshProUGUI ProgressText { get; private set; }
        
        [field: SerializeField]
        public OrderProgress OrderProgress { get; private set; }

        private void OnEnable()
        {
            OrderProgress.OrderProgressChanged.AddListener(UpdateProgress);
        }

        private void OnDisable()
        {
            OrderProgress.OrderProgressChanged.RemoveListener(UpdateProgress);
        }

        private void UpdateProgress()
        {
            var scale = Progress.maxValue / OrderProgress.CurrentOrder.InitialGoal;
            var scaledValue = OrderProgress.CurrentOrder.Goal * scale;
            Progress.value = Mathf.Clamp(scaledValue, Progress.minValue, Progress.maxValue);
            ProgressText.text = OrderProgress.CurrentOrder.Goal.ToString();
        }
    }
}