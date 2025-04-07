using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Game.Scripts.Levels
{
    public class Level : LevelBase
    {
    }

    public abstract class LevelBase : MonoBehaviour
    {
        private bool _initialized = false;

        public UnityEvent<LevelBase> LevelEnabled;
        [SerializeField] 
        private List<Artifact> _artifacts;

        [field: SerializeField]
        public int LevelNumber { get; private set; }
        
        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }

        public IReadOnlyCollection<Artifact> Artifacts => _artifacts;

        [field: SerializeField]
        public Tilemap FloorTilemap { get; private set; }
        
        [field: SerializeField]
        public Tilemap WallsTilemap { get; private set; }
        
        public void Init(IReadOnlyCollection<Artifact> artifacts, int levelNumber, float oxygenConsumptionRate)
        {
            if (_initialized)
            {
                Debug.LogError("Level cannot be initialized twice");
                return;
            }

            _artifacts = artifacts.ToList();
            LevelNumber = levelNumber;
            OxygenConsumptionRate = oxygenConsumptionRate;
            
            _initialized = true;
        }

        public void AddArtifact(Artifact artifact)
        {
            _artifacts.Add(artifact);
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            LevelEnabled?.Invoke(this);
        }

        private void OnDestroy()
        {
            LevelEnabled?.RemoveAllListeners();
        }
    }
}
