using System;
using System.Collections.Generic;
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
        [field: SerializeField]
        public UnityEvent<Artifact> OnArtifactSpawned { get; private set; }

        [FormerlySerializedAs("_database")] [SerializeField]
        private ArtifactsDatabase _artifactsDatabase;

        [field: SerializeField]
        public Artifact ArtifactPrefab { get; private set; }

        [field: SerializeField]
        public Artifact LightArtifactPrefab { get; private set; }

        [field: SerializeField]
        public LevelSettingsDatabase LevelSettingsDatabase { get; private set; }

        [field: SerializeField]
        public LevelSwitcher LevelSwitcher { get; private set; }

        public Artifact SpawnArtifact(Vector3 pos, LevelBase level)
        {
            var artifactData = GetRandomArtifactData(level);
            var artifactPrefab = artifactData.ArtifactId == ArtifactId.Firefly
                ? LightArtifactPrefab
                : ArtifactPrefab;

            var artifact = Instantiate(artifactPrefab, pos, Quaternion.identity, level.transform);
            artifact.Collected.AddListener(level.Remove);
            artifactData.SpawnedAtLevel = level.LevelNumber;
            artifactData.Position = pos;
            artifact.Init(artifactData);
            OnArtifactSpawned?.Invoke(artifact);
            Debug.Log($"Spawn artifact: {artifact.Data.ArtifactId}");
            return artifact;
        }

        public void DropArtifact(Vector3 transformPosition, ArtifactData artifactData, LevelBase level)
        {
            var artifactPrefab = artifactData.ArtifactId == ArtifactId.Firefly
                ? LightArtifactPrefab
                : ArtifactPrefab;

            var artifact = Instantiate(artifactPrefab, transformPosition, Quaternion.identity, level.transform);
            artifact.Collected.AddListener(level.Remove);
            artifactData.Position = transformPosition;
            artifact.Init(artifactData);
            if (artifactData.IsRevealed)
            {
                artifact.Reveal();
            }

            Debug.Log($"Drop artifact: {artifact.Data.ArtifactId}");
            OnArtifactSpawned?.Invoke(artifact);
        }

        public Artifact DropArtifact(Vector3 transformPosition, ArtifactData artifactData)
        {
            var level = LevelSwitcher.CurrentLevel;
            var artifactPrefab = artifactData.ArtifactId == ArtifactId.Firefly
                ? LightArtifactPrefab
                : ArtifactPrefab;

            var artifact = Instantiate(artifactPrefab, transformPosition, Quaternion.identity, level.transform);
            artifact.Collected.AddListener(level.Remove);
            artifactData.Position = transformPosition;
            artifact.Init(artifactData);
            if (artifactData.IsRevealed)
            {
                artifact.Reveal();
            }

            level.AddArtifacts(artifact.Data);
            Debug.Log($"Drop artifact: {artifact.Data.ArtifactId}");
            OnArtifactSpawned?.Invoke(artifact);
            return artifact;
        }

        private ArtifactData GetRandomArtifactData(LevelBase level)
        {
            Debug.Log($"Start spawning artifact on level: {level.LevelNumber}, {level.LevelType}");

            var levelArtifacts = _artifactsDatabase.Get(Math.Abs(level.LevelNumber), level.LevelType);

            if (level.LevelType == LevelType.Unique)
            {
                var artifactPrefab = GetRandomArtifact(levelArtifacts, ArtifactRarity.Unique);
                return new ArtifactData(artifactPrefab);
            }

            var notUniqueArtifacts = levelArtifacts
                .Where(x => x.Rarity != ArtifactRarity.Unique)
                .ToArray();

            var allRarities = notUniqueArtifacts
                .Select(x => x.Rarity)
                .Distinct()
                .ToArray();

            var selectedRarity = allRarities.Length switch
            {
                1 => allRarities[0],
                _ => Random.value > 0.8f ? ArtifactRarity.Rare : ArtifactRarity.Common
            };

            var randomArtifactPrefab = GetRandomArtifact(notUniqueArtifacts, selectedRarity);
            return new ArtifactData(randomArtifactPrefab);
        }

        private ArtifactDataSO GetRandomArtifact(IEnumerable<ArtifactDataSO> artifacts, ArtifactRarity rarity)
        {
            return artifacts
                .Where(x => x.Rarity == rarity)
                .OrderBy(_ => Random.value)
                .FirstOrDefault();
        }
    }
}