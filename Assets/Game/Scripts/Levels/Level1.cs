using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Levels
{
    public class Level1 : MonoBehaviour
    {
        [SerializeField] private List<Artifact> _artifacts = new();

        public IReadOnlyCollection<Artifact> Artifacts => _artifacts;
        
        public UnityEvent<Level1> LevelChanged;
        
        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Зашел на левел 1!");
                var player = other.GetComponent<Player>();
                player.OxygenConsumptionRate = OxygenConsumptionRate;
                LevelChanged?.Invoke(this);
            }
        }
        
        public void OnArtifactSpawned(Artifact artifact)
        {
            _artifacts.Add(artifact);
        }

        private void OnDestroy()
        {
            LevelChanged?.RemoveAllListeners();
        }
    }
}