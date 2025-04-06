using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class ArtifactSpawner : MonoBehaviour
    {
        [field: SerializeField]
        public Artifact ArtifactPrefab { get; set; }

        [field: SerializeField]
        public UnityEvent<Artifact> OnArtifactSpawned { get; set; }

        public Artifact SpawnArtifact(Vector3 pos, Transform levelTransform)
        {
            var artifact = Instantiate(ArtifactPrefab, pos, Quaternion.identity, levelTransform);
            OnArtifactSpawned?.Invoke(artifact);
            return artifact;
        }
    }
}