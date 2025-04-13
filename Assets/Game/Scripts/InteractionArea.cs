using System;
using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class InteractionArea : MonoBehaviour
    {
        public UnityEvent PlayerEntered;
        public UnityEvent PlayerExited;
        public UnityEvent<ArtifactData> ArtifactPlaced;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerEntered?.Invoke();
            }
            
            if (other.TryGetComponent<Artifact>(out var artifact))
            {
                Debug.Log("Артифакт размещен в контейнере!");
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                // CompleteOrder(new[] { artifact.Data });
                ArtifactPlaced?.Invoke(artifact.Data);
                _spriteRenderer.sprite = artifact.Data.Sprite;
                // OrderCompleted?.Invoke(artifact.Data.GetFinalPrice());
                Destroy(artifact.gameObject);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerExited?.Invoke();
            }
        }
    }
}