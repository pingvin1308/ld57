using Game.Scripts.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Artifacts
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

        [field: SerializeField]
        public LevelSettingsDatabase LevelSettingsDatabase { get; private set; } 

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
            var randomArtifactId = LevelSettingsDatabase.GetRandomArtifactId(levelNumber);
            var artifactData = _database.GetByID(randomArtifactId);
            return artifactData;
        }
    }
}