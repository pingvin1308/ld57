using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Artifacts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Artifact : MonoBehaviour
    {
        [field: SerializeField]
        public Guid Id { get; private set; } = Guid.NewGuid();
        
        private SpriteRenderer _spriteRenderer;
        
        [field: SerializeField]
        public bool IsPickable { get; private set; }

        public UnityEvent<ArtifactData> Collected;

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
            Data.IsRevealed = false;
            _spriteRenderer.enabled = false;
            IsPickable = false;
        }

        private IEnumerator SetPickable()
        {
            yield return new WaitForSeconds(0.5f);
            Data.IsRevealed = true;
            IsPickable = true;
        }
        
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (_isPickable == false)
        //     {
        //         return;
        //     }
        //
        //     if (other.TryGetComponent<Player>(out var player))
        //     {
        //         // Здесь можно добавить звук, эффект, счетчик и т.д.
        //         if (player.Inventory.IsEnoughSpace)
        //         {
        //             Debug.Log("Предмет подобран!");
        //             Collected?.Invoke(Data);
        //             Collected?.RemoveAllListeners();
        //             Destroy(gameObject);
        //         }
        //     }
        // }

        private void OnDestroy()
        {
            Collected?.RemoveAllListeners();
        }
    }
}