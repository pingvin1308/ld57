using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Levels
{
    public class UpLevelTrigger : MonoBehaviour
    {
        public UnityEvent<LevelDirection> LevelChanged;

        [field: SerializeField]
        public DownLevelTrigger DownLevelTrigger { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                if (player.CurrentLevel is ExpeditionHub)
                {
                    // player.transform.position 
                    Debug.Log("test");
                    return;
                }

                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Зашел на левел выше");
                LevelChanged?.Invoke(LevelDirection.Up);
                player.transform.position = DownLevelTrigger.transform.position + new Vector3(0, 1);
            }
        }

        private void OnDestroy()
        {
            LevelChanged?.RemoveAllListeners();
        }
    }
}