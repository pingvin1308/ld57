using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Levels
{
    public class DownLevelTrigger : MonoBehaviour
    {
        [SerializeField] private List<Artifact> _artifacts = new();

        public IReadOnlyCollection<Artifact> Artifacts => _artifacts;
        
        public UnityEvent<LevelDirection> LevelChanged;
        
        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Здесь можно добавить звук, эффект, счетчик и т.д.
                Debug.Log("Зашел на левел ниже");
                LevelChanged?.Invoke(LevelDirection.Down);
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