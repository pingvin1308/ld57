using System.Collections.Generic;
using System.Linq;
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
            if (CurrentOrder.TryComplete(artifacts))
            {
                OrderCompleted?.Invoke(CurrentOrder.Reward);
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                Debug.Log("Игрок подошел!");
            }
            
            if (other.TryGetComponent<Artifact>(out var artifact))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                // обмениваем артифакты на деньги
                // var player = other.GetComponent<Player>();
                CompleteOrder(new[] { artifact.Data });
                // player.Inventory.DropArtifacts();

                // просчитать ценность
                // var totalPrice = artifacts * 100;
                // player.Inventory.AddMoney(totalPrice);
                // artifact.de
                Destroy(artifact.gameObject);

                Debug.Log("Артифакты проданы!");
            }
        }
    }

    public class OrderData
    {
        /// <summary>
        /// Sum of artifacts value
        /// </summary>
        public int InitialGoal { get; }
        
        /// <summary>
        /// Sum of artifacts value
        /// </summary>
        public int Goal { get; private set; }

        /// <summary>
        /// Money
        /// </summary>
        public int Reward { get; private set; }

        public OrderData(int goal, int reward)
        {
            InitialGoal = goal;
            Goal = goal;
            Reward = reward;
        }

        public bool TryComplete(IReadOnlyCollection<ArtifactData> artifacts)
        {
            Goal -= artifacts.Sum(x => x.Value);
            return Goal <= 0;
        }
    }

    public class OrderProgression
    {
        public int CompletedOrders { get; set; }

        public OrderData GetNext()
        {
            var minScale = CompletedOrders + 1;
            return new OrderData(10 * minScale, 10 * minScale);
        }
    }
}