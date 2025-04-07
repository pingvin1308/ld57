using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class Trade : MonoBehaviour
    {
        public UnityEvent<int> OrderCompleted;
        public UnityEvent<int> OrderChanged;

        [SerializeField] private Order _currentOrder;
        
        public Order CurrentOrder
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
        }

        public void CompleteOrder(IReadOnlyCollection<ArtifactData> artifacts)
        {
            if (CurrentOrder.Complete(artifacts))
            {
                OrderCompleted?.Invoke(CurrentOrder.Reward);
                Progression.CompletedOrders++;
                CurrentOrder = Progression.GetNext();
            }
        }

        private void OnDestroy()
        {
            OrderCompleted?.RemoveAllListeners();
            OrderChanged?.RemoveAllListeners();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                // обмениваем артифакты на деньги
                var player = other.GetComponent<Player>();
                CompleteOrder(player.Inventory.CollectedArtifacts);
                player.Inventory.DropArtifacts();

                // просчитать ценность
                // var totalPrice = artifacts * 100;
                // player.Inventory.AddMoney(totalPrice);

                Debug.Log("Артифакты проданы!");
            }
        }
    }
    
        
    public class Order
    {
        /// <summary>
        /// Sum of artifacts value
        /// </summary>
        public int Goal { get; private set; }
        
        /// <summary>
        /// Money
        /// </summary>
        public int Reward { get; private set; }

        public Order(int goal, int reward)
        {
            Goal = goal;
            Reward = reward;
        }
        
        public bool Complete(IReadOnlyCollection<ArtifactData> artifacts)
        {
            Goal -= artifacts.Sum(x => x.Value);
            return Goal <= 0;
        }
    }

    public class OrderProgression
    {
        public int CompletedOrders { get; set; }
        
        public Order GetNext()
        {
            var minScale = CompletedOrders + 1;
            return new Order(10 * minScale, 10 * minScale);
        }
    }
}
