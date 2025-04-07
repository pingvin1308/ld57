using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class ArtifactSpawner : MonoBehaviour
    {
        [SerializeField] 
        private ArtifactsDatabase _database;
        
        [field: SerializeField]
        public Artifact ArtifactPrefab { get; set; }

        [field: SerializeField]
        public UnityEvent<Artifact> OnArtifactSpawned { get; set; }

        [field: SerializeField]
        public Player Player { get; private set; }
        
        public Artifact SpawnArtifact(Vector3 pos, Transform levelTransform)
        {
            var artifact = Instantiate(ArtifactPrefab, pos, Quaternion.identity, levelTransform);
            var artifactIds = Enum.GetValues(typeof(ArtifactId));
            var randomArtifactId = (ArtifactId)artifactIds.GetValue(Random.Range(0, artifactIds.Length));
            var artifactData = _database.GetByID(randomArtifactId);
            artifact.Init(artifactData);
            OnArtifactSpawned?.Invoke(artifact);
            return artifact;
        }

        public void DropArtifact(ArtifactData artifactData)
        {
            var artifact = Instantiate(ArtifactPrefab, Player.transform.position, Quaternion.identity, Player.CurrentLevel.transform);
            artifact.Init(artifactData);
            Player.CurrentLevel.AddArtifact(artifact);
            OnArtifactSpawned?.Invoke(artifact);
        }
    }
}