using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class ScanArea : MonoBehaviour
    {
        public UnityEvent DistanceChanged;
        public UnityEvent ArtifactsDetected;
        [SerializeField] private Artifact _nearestArtifact;
        [SerializeField] private List<Artifact> _scannedArtifacts = new();
        [SerializeField] private bool _artifactDetected;
        [SerializeField] private float distanceToNearestArtifact;

        public float DistanceToNearestArtifact
        {
            get => distanceToNearestArtifact;
            private set
            {
                if (!Mathf.Approximately(distanceToNearestArtifact, value))
                {
                    DistanceChanged?.Invoke();
                }

                distanceToNearestArtifact = value;
            }
        }

        public bool ArtifactDetected
        {
            get => _artifactDetected;
            private set
            {
                _artifactDetected = value;
                ArtifactsDetected?.Invoke();
            }
        }

        private void Update()
        {
            _nearestArtifact = _scannedArtifacts
                .Where(x => x != null)
                .Where(x => x.Data.Type != ArtifactType.Currency)
                .Select(x => new
                {
                    Distance = Vector3.Distance(transform.position, x.transform.position),
                    Artifact = x
                })
                .OrderBy(x => x.Distance)
                .FirstOrDefault()
                ?.Artifact;

            if (_nearestArtifact != null)
            {
                DistanceToNearestArtifact = Vector3.Distance(transform.position, _nearestArtifact.transform.position);
                ArtifactDetected = true;
            }
            else
            {
                DistanceToNearestArtifact = 0;
                ArtifactDetected = false;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent<Artifact>(out var artifact) &&
                artifact.Data.AcquisitionMethod == AcquisitionMethod.Detector)
            {
                _scannedArtifacts.Add(artifact);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Artifact>(out var artifact) &&
                artifact.Data.AcquisitionMethod == AcquisitionMethod.Detector)
            {
                _scannedArtifacts.Remove(artifact);
            }
        }

        private void OnDestroy()
        {
            DistanceChanged?.RemoveAllListeners();
            ArtifactsDetected?.RemoveAllListeners();
        }
    }
}