using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Game.Scripts.Levels
{
    [Serializable]
    public class LevelData
    {
        public int Seed { get; private set; }

        public int LevelNumber { get; private set; }

        public List<ArtifactData> Artifacts { get; private set; } = new();

        public LevelType LevelType { get; private set; }

        public float OxygenConsumptionRate { get; private set; }

        public LevelData(int seed, int levelNumber, LevelType levelType, float oxygenConsumptionRate)
        {
            Seed = seed;
            LevelNumber = levelNumber;
            LevelType = levelType;
            OxygenConsumptionRate = oxygenConsumptionRate;
        }
    }

    public abstract class LevelBase : MonoBehaviour
    {
        private bool _initialized;

        public UnityEvent<LevelBase> LevelEnabled;

        [field: SerializeField]
        public Guid Id { get; private set; } = Guid.NewGuid();
        
        [Header("State")]
        public LevelData Data { get; private set; }

        public IReadOnlyCollection<ArtifactData> Artifacts => Data.Artifacts;

        public int LevelNumber => Data.LevelNumber;

        public LevelType LevelType => Data.LevelType;

        public float OxygenConsumptionRate => Data.OxygenConsumptionRate;

        [Header("Dependencies")]
        public Player Player { get; private set; }

        [field: SerializeField]
        public Tilemap FloorTilemap { get; private set; }

        [field: SerializeField]
        public Tilemap WallsTilemap { get; private set; }

        public void Init(LevelData data, ArtifactData[] artifacts = null)
        {
            if (_initialized)
            {
                Debug.LogError("Level cannot be initialized twice");
                return;
            }

            Data = data;

            if (artifacts is { Length: > 0 })
            {
                AddArtifacts(artifacts);
            }

            _initialized = true;
        }

        // private void OnEnable()
        // {
        //     Player.Inventory.OnInventoryChanged.AddListener(Remove);
        // }
        //
        // private void OnDisable()
        // {
        //     Player.Inventory.OnInventoryChanged.RemoveListener(Remove);
        // }

        public void AddArtifacts(params ArtifactData[] artifacts)
        {
            Data.Artifacts.AddRange(artifacts);
        }

        public void Remove(ArtifactData artifactData)
        {
            Data.Artifacts.Remove(artifactData);
        }

        public virtual void Disable()
        {
            // Player.Inventory.OnInventoryChanged.RemoveListener(Remove);
            Player = null;
            // gameObject.SetActive(false);
            // foreach (var artifact in Artifacts)
            // {
            // artifact.gameObject.SetActive(false);
            // }
        }

        public void Enable(Player player)
        {
            Player = player;
            Player.OnLevelEnabled(this);
            gameObject.SetActive(true);
            LevelEnabled?.Invoke(this);
            // Player.Inventory.OnInventoryChanged.AddListener(Remove);

            // foreach (var artifact in Artifacts)
            // {
            // artifact.gameObject.SetActive(true);
            // }
        }

        private void OnDestroy()
        {
            LevelEnabled?.RemoveAllListeners();
        }
    }
}