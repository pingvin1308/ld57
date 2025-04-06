using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class Artifact : MonoBehaviour
    {
        public UnityEvent<ArtifactData> Collected;
    
        [field: SerializeField]
        public ArtifactData Data { get; private set; }

        [field: SerializeField]
        public Rarity Rarity { get; private set; }

        public void Awake()
        {
            Rarity = Rarity.Common;
            Data = new ArtifactData
            {
                Rarity = Rarity.Common,
                Value = 10
            };
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Предмет подобран!");
                Collected?.Invoke(Data);
                Destroy(gameObject); // Удалить кружок с сцены
            }
        }

        private void OnDestroy()
        {
            Collected?.RemoveAllListeners();
        }
    }

    public class ArtifactData
    {
        public int Value { get; set; }
        
        public Rarity Rarity { get; set; }
    }

    public enum Rarity
    {
        Common = 0,
        Rare = 1,
        Unique = 2,
    }
}