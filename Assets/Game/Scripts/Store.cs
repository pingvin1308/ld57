using UnityEngine;

namespace Game.Scripts
{
    public class Store : MonoBehaviour
    {
        [field: SerializeField]
        public int Upgrade { get; private set; }

        public Upgrades Upgrades => new()
        {
            OxygenUpgrades = new[]
            {
                (Price: 0, Value: 0),
                (Price: 100, Value: 300),
                (Price: 500, Value: 1500),
                (Price: 1500, Value: 6300)
                // ( 4, 400 ),
                // ( 5, 500 ),
            },
            DetectorUpgrades = new[]
            {
                (Price: 0, Value: 0),
                (Price: 1, Value: 1),
                (Price: 2, Value: 2),
                (Price: 3, Value: 3)
                // ( 4, 400 ),
                // ( 5, 500 ),
            },
            InventoryUpgrades = new[]
            {
                (Price: 0, Value: 0),
                (Price: 1, Value: 1),
                (Price: 2, Value: 2),
                (Price: 3, Value: 3)
                // ( 4, 400 ),
                // ( 5, 500 ),
            },
        };

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Предмет подобран!");

                // обмениваем артифакты на деньги
                var player = other.GetComponent<Player>();
                var upgradeLevel = player.Oxygen.UpgradeLevel + 1;
                var upgrade = Upgrades.OxygenUpgrades[upgradeLevel];

                // просчитать ценность
                if (player.Inventory.SpendMoney(upgrade.Price))
                {
                    player.Oxygen.ApplyUpgrade(upgrade.Value, upgradeLevel);
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
        public (int Price, int Value)[] DetectorUpgrades { get; set; }

        /// <summary>
        /// Key: level
        /// Value: Volume
        /// </summary>
        public (int Price, int Value)[] OxygenUpgrades { get; set; }

        /// <summary>
        /// Key: level
        /// Value: Slot
        /// </summary>
        public (int Price, int Value)[] InventoryUpgrades { get; set; }
    }
}