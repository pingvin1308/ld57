using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Levels
{
    public class DownLevelTrigger : MonoBehaviour
    {
        public UnityEvent<LevelDirection> LevelChanged;
        
        [field: SerializeField]
        public UpLevelTrigger UpLevelTrigger { get; private set; }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Зашел на левел ниже");
                LevelChanged?.Invoke(LevelDirection.Down);

                var player = other.GetComponent<Player>();
                player.transform.position = UpLevelTrigger.transform.position + new Vector3(0, -3);
            }
        }

        private void OnDestroy()
        {
            LevelChanged?.RemoveAllListeners();
        }
    }
}