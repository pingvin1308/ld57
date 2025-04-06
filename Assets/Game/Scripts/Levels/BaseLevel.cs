using UnityEngine;

namespace Game.Scripts.Levels
{
    public class BaseLevel : MonoBehaviour
    {
        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Зашел на базу!");
                var player = other.GetComponent<Player>();
                player.OxygenConsumptionRate = OxygenConsumptionRate;
                player.Oxygen.Restore();
            }
        }
    }
}