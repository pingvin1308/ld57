using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Levels
{
    public class UpLevelTrigger : MonoBehaviour
    {
        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }
        public UnityEvent<LevelDirection> LevelChanged;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Зашел на левел выще");
                LevelChanged?.Invoke(LevelDirection.Up);
            }
        }

        private void OnDestroy()
        {
            LevelChanged?.RemoveAllListeners();
        }
    }
}