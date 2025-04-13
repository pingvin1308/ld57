using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Artifacts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Artifact : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private bool _isPickable;

        public UnityEvent<ArtifactData> Collected;

        [field: SerializeField]
        public bool IsRevealed { get; private set; }

        [field: SerializeField]
        public ArtifactData Data { get; private set; }

        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(ArtifactData data)
        {
            Data = data;
            _spriteRenderer.sprite = data.Sprite;

            if (Data.Type == ArtifactType.Currency)
            {
                Reveal();
            }
            else
            {
                _spriteRenderer.enabled = false;
            }
        }
        
        public void Reveal()
        {
            _spriteRenderer.enabled = true;
            StartCoroutine(SetPickable());
        }

        public void Hide()
        {
            _spriteRenderer.enabled = false;
            IsRevealed = false;
            _isPickable = false;
        }

        private IEnumerator SetPickable()
        {
            yield return new WaitForSeconds(0.5f);
            IsRevealed = true;
            _isPickable = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPickable == false)
            {
                return;
            }

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
}