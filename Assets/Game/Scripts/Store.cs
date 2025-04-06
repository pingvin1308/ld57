using UnityEngine;

namespace Game.Scripts
{
    public class Store : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Предмет подобран!");
            
                // обмениваем артифакты на деньги
                var player = other.GetComponent<Player>();
            
                // просчитать ценность
                var upgradePrice = 100;
                if (player.Inventory.SpendMoney(upgradePrice))
                {
                    player.Oxygen.ApplyUpgrade(0.2f);
                    Debug.Log("Апгрейд куплен!");
                }
                else
                {
                    Debug.Log("Не хватает денех!");
                }
            }
        }
    }
}