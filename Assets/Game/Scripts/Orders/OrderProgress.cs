using System.Collections.Generic;
using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class OrderProgress : MonoBehaviour
    {
        public UnityEvent<int> OrderCompleted;
        public UnityEvent<int> OrderChanged;
        public UnityEvent OrderProgressChanged;

        [SerializeField] private OrderData _currentOrder;

        public OrderData CurrentOrder
        {
            get => _currentOrder;
            private set
            {
                _currentOrder = value;
                OrderChanged?.Invoke(_currentOrder.Goal);
            }
        }

        [field: SerializeField]
        public OrderProgression Progression { get; private set; }

        private void Awake()
        {
            Progression = new OrderProgression();
            CurrentOrder = Progression.GetNext();
            OrderProgressChanged?.Invoke();
        }

        public void CompleteOrder(IReadOnlyCollection<ArtifactData> artifacts)
        {
            foreach (var artifact in artifacts)
            {
                var price = artifact.GetFinalPrice();
                Debug.Log($"OrderProgress: Sold artifact {artifact.ArtifactId} for price {price}");
                OrderCompleted?.Invoke(price);
                Progression.CompletedOrders++;
                CurrentOrder = Progression.GetNext();
            }
            
            OrderProgressChanged?.Invoke();
        }

        private void OnDestroy()
        {
            OrderCompleted?.RemoveAllListeners();
            OrderChanged?.RemoveAllListeners();
            OrderProgressChanged?.RemoveAllListeners();
        }
    }
}