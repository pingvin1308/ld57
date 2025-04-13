using System;
using System.Linq;
using Game.Scripts.Levels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Scripts.Artifacts
{
    public class ArtifactSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("_database")] [SerializeField]
        private ArtifactsDatabase _artifactsDatabase;

        [field: SerializeField]
        public Artifact ArtifactPrefab { get; set; }

        [field: SerializeField]
        public UnityEvent<Artifact> OnArtifactSpawned { get; set; }

        [field: SerializeField]
        public Player Player { get; private set; }

        [field: SerializeField]
        public LevelSettingsDatabase LevelSettingsDatabase { get; private set; }

        public Artifact SpawnArtifact(Vector3 pos, LevelBase level)
        {
            var artifact = Instantiate(ArtifactPrefab, pos, Quaternion.identity, level.transform);
            var artifactData = GetRandomArtifactData(level);
            artifactData.SpawnedAtLevel = level.LevelNumber;
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

        private ArtifactData GetRandomArtifactData(LevelBase level)
        {
            Debug.Log($"Start spawning artifact on level: {level.LevelNumber}, {level.LevelType}");

            var levelArtifacts = _artifactsDatabase.Get(Math.Abs(level.LevelNumber), level.LevelType);

            if (level.LevelType == LevelType.Unique)
            {
                // генерируем один на игру
                return levelArtifacts
                    .Where(x => x.Rarity == ArtifactRarity.Rare)
                    .OrderBy(_ => Random.value)
                    .First();
            }

            var notUniqueArtifacts = levelArtifacts
                .Where(x => x.Rarity != ArtifactRarity.Unique)
                .ToArray();

            if (notUniqueArtifacts.All(x => x.Rarity == ArtifactRarity.Common))
            {
                return notUniqueArtifacts
                    .Where(x => x.Rarity == ArtifactRarity.Common)
                    .OrderBy(_ => Random.value)
                    .First();
            }

            if (notUniqueArtifacts.All(x => x.Rarity == ArtifactRarity.Rare))
            {
                return notUniqueArtifacts
                    .Where(x => x.Rarity == ArtifactRarity.Rare)
                    .OrderBy(_ => Random.value)
                    .First();
            }

            // Common - 0.8
            // Rare - 0.2
            var rarity = Random.value > 0.8
                ? ArtifactRarity.Rare
                : ArtifactRarity.Common;

            var randomArtifact = notUniqueArtifacts
                .Where(x => x.Rarity == rarity)
                .OrderBy(_ => Random.value)
                .First();

            Debug.Log($"Spawn artifact: {randomArtifact.ArtifactId}");

            return Instantiate(randomArtifact);            
        }
    }
}