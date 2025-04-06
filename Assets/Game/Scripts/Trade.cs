using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class Trade : MonoBehaviour
    {
        public UnityEvent<int> OrderCompleted;
        
        [field: SerializeField]
        public Order Order { get; private set; }

        private void Awake()
        {
            Order = new Order
            {
                Reward = 10
            };
        }

        public void CompleteOrder(IReadOnlyCollection<ArtifactData> artifacts)
        {
            Order.Complete(artifacts);
            OrderCompleted?.Invoke(Order.Reward);
        }

        private void OnDestroy()
        {
            OrderCompleted?.RemoveAllListeners();
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
}
