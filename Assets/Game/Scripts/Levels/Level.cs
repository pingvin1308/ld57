using System;
using System.Collections.Generic;
using Game.Scripts.Artifacts;
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
        private List<Artifact> _artifacts = new();

        [field: SerializeField] 
        public int LevelNumber { get; private set; }

        [field: SerializeField]
        public LevelType LevelType { get; private set; }
        
        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }

        public IReadOnlyCollection<Artifact> Artifacts => _artifacts;

        [field: SerializeField]
        public Tilemap FloorTilemap { get; private set; }
        
        [field: SerializeField]
        public Tilemap WallsTilemap { get; private set; }
        
        public void Init(int levelNumber, float oxygenConsumptionRate, IReadOnlyCollection<Artifact> artifacts = null)
        {
            if (_initialized)
            {
                Debug.LogError("Level cannot be initialized twice");
                return;
            }

            if (artifacts is { Count: > 0 })
            {
                _artifacts.AddRange(artifacts);
            }

            LevelNumber = levelNumber;
            OxygenConsumptionRate = oxygenConsumptionRate;
            
            _initialized = true;
        }

        public void AddArtifacts(params Artifact[] artifacts)
        {
            _artifacts.AddRange(artifacts);
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
