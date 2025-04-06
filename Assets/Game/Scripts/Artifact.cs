using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class Artifact : MonoBehaviour
    {
        public UnityEvent<int> Collected;
    
        public Rarity Rarity { get; private set; }

        public void Awake()
        {
            Rarity = Rarity.Common;
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Предмет подобран!");
                Collected?.Invoke(1);
                Destroy(gameObject); // Удалить кружок с сцены
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