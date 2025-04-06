using UnityEngine;

namespace Game.Scripts
{
    public class Trade : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                // обмениваем артифакты на деньги
                var player = other.GetComponent<Player>();
                var artifacts = player.Inventory.DropArtifacts();
            
                // просчитать ценность
                var totalPrice = artifacts * 100;
                player.Inventory.AddMoney(totalPrice);

                Debug.Log("Артифакты проданы!");
            }
        }
    }
}
