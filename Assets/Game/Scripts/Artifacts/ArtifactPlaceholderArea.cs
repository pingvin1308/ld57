using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ArtifactPlaceholderArea : MonoBehaviour
    {
        public UnityEvent PlayerEntered;
        public UnityEvent PlayerExited;
        public UnityEvent<ArtifactData> ArtifactPlaced;

        private SpriteRenderer _spriteRenderer;

        [field: SerializeField]
        public Player Player { get; private set; }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                Player = player;
                PlayerEntered?.Invoke();
            }

            if (other.TryGetComponent<Artifact>(out var artifact))
            {
                ArtifactPlaced?.Invoke(artifact.Data);
                Destroy(artifact.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                PlayerExited?.Invoke();
                Player = null;
            }
        }

        public void UpdateUI(ArtifactData artifact)
        {
            _spriteRenderer.sprite = artifact?.Sprite;
        }

        public void CleanUI()
        {
            _spriteRenderer.sprite = null;
        }

        public void DropArtifact(ArtifactData artifact)
        {
            Player.Inventory.OnArtifactCollected(artifact);
        }
    }
}