using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Artifact : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        public UnityEvent<ArtifactData> Collected;
    
        [field: SerializeField]
        public ArtifactData Data { get; private set; }

        [field: SerializeField]
        public Rarity Rarity { get; private set; }

        
        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Rarity = Rarity.Common;
        }

        public void Init(ArtifactData data)
        {
            Data = data;
            _spriteRenderer.sprite = data.Sprite;
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                var player = other.GetComponent<Player>();
                if (player.Inventory.IsEnoughSpace)
                {
                    Debug.Log("Предмет подобран!");
                    Collected?.Invoke(Data);
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            Collected?.RemoveAllListeners();
        }
    }

    public enum Rarity
    {
        Common = 0,
        Rare = 1,
        Unique = 2,
    }
}