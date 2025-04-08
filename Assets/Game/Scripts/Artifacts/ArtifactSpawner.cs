using System;
using System.Collections.Generic;
using Game.Scripts.Levels;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class ArtifactSpawner : MonoBehaviour
    {
        [SerializeField] private ArtifactsDatabase _database;

        [field: SerializeField]
        public Artifact ArtifactPrefab { get; set; }

        [field: SerializeField]
        public UnityEvent<Artifact> OnArtifactSpawned { get; set; }

        [field: SerializeField]
        public Player Player { get; private set; }

        public ArtifactSettings ArtifactSettings { get; set; } = new ArtifactSettings
        {
            ArtifactIds = new Dictionary<int, ArtifactId[]>
            {
                { 1, new[] { ArtifactId.Sneakers } },
                { 2, new[] { ArtifactId.LightWeight } },
                { 3, new[] { ArtifactId.GreedyCoin } },
                { 4, new[] { ArtifactId.MovementMirror } },
                { 5, new[] { ArtifactId.SlippyBanana } },
                { 6, new[] { ArtifactId.ErosionMud } },
                { 7, new[] { ArtifactId.StinkyCheese } },
            }
        };

        public Artifact SpawnArtifact(Vector3 pos, LevelBase level)
        {
            var artifact = Instantiate(ArtifactPrefab, pos, Quaternion.identity, level.transform);
            var artifactData = GetRandomArtifactData(level.LevelNumber);
            artifact.Init(artifactData);
            OnArtifactSpawned?.Invoke(artifact);
            return artifact;
        }

        public void DropArtifact(ArtifactData artifactData)
        {
            var artifact = Instantiate(ArtifactPrefab, Player.transform.position, Quaternion.identity,
                Player.CurrentLevel.transform);
            artifact.Init(artifactData);
            artifact.Reveal();
            Player.CurrentLevel.AddArtifacts(artifact);
            OnArtifactSpawned?.Invoke(artifact);
        }

        private ArtifactData GetRandomArtifactData(int levelNumber)
        {
            var artifactIds = ArtifactSettings.ArtifactIds[Math.Abs(levelNumber)];
            var randomArtifactId = artifactIds[Random.Range(0, artifactIds.Length)];
            var artifactData = _database.GetByID(randomArtifactId);
            return artifactData;
        }
    }

    public class ArtifactSettings
    {
        /// <summary>
        /// Key: LevelNumber
        /// Value: Possible artifacts on the level 
        /// </summary>
        public Dictionary<int, ArtifactId[]> ArtifactIds { get; set; }
    }
}