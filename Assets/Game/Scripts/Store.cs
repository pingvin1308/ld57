using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class Store : MonoBehaviour
    {
        [field: SerializeField]
        public int Price { get; private set; }

        [field: SerializeField]
        public int Upgrade { get; private set; }

        public Upgrades Upgrades => new()
        {
            OxygenUpgrades = new Dictionary<int, float>
            {
                { 1, 300 },
                { 2, 350 },
                { 3, 300 },
                { 4, 400 },
                { 5, 500 },
            }
        };

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Предмет подобран!");

                // обмениваем артифакты на деньги
                var player = other.GetComponent<Player>();

                // просчитать ценность
                if (player.Inventory.SpendMoney(Price))
                {
                    var upgradeLevel = player.Oxygen.UpgradeLevel + 1;
                    var upgrade = Upgrades.OxygenUpgrades[upgradeLevel];
                    player.Oxygen.ApplyUpgrade(upgrade, upgradeLevel);
                    Debug.Log("Апгрейд куплен!");
                }
                else
                {
                    Debug.Log("Не хватает денех!");
                }
            }
        }
    }

    public class Upgrades
    {
        /// <summary>
        /// Key: level
        /// Value: Range
        /// </summary>
        public Dictionary<int, int> DetectorUpgrades { get; set; }

        /// <summary>
        /// Key: level
        /// Value: Volume
        /// </summary>
        public Dictionary<int, float> OxygenUpgrades { get; set; }

        /// <summary>
        /// Key: level
        /// Value: Slot
        /// </summary>
        public Dictionary<int, int> InventoryUpgrades { get; set; }
    }
}