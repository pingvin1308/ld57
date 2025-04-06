using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class Radar : MonoBehaviour
    {
        public UnityEvent<float, float> DistanceChanged;
        public UnityEvent<bool> ArtifactsDetected;
        [SerializeField] private float distanceToNearestArtifact;

        [field: SerializeField]
        public Artifact NearestArtifact { get; private set; }

        [field: SerializeField]
        public float Range { get; private set; }

        public float DistanceToNearestArtifact
        {
            get => distanceToNearestArtifact;
            private set
            {
                if (!Mathf.Approximately(distanceToNearestArtifact, value))
                {
                    DistanceChanged?.Invoke(Range, distanceToNearestArtifact);
                }
                
                distanceToNearestArtifact = value;
            }
        }

        public void Scan(IReadOnlyCollection<Artifact> artifacts)
        {
            NearestArtifact = artifacts
                .Where(x => x != null)
                .Select(x => new
                {
                    Distance = Vector3.Distance(transform.position, x.transform.position),
                    Artifact = x
                })
                .Where(x => x.Distance < Range)
                .OrderBy(x => x.Distance)
                .FirstOrDefault()
                ?.Artifact;

            if (NearestArtifact != null)
            {
                ArtifactsDetected?.Invoke(true);
                DistanceToNearestArtifact = Vector3.Distance(transform.position, NearestArtifact.transform.position);
            }
            else
            {
                ArtifactsDetected?.Invoke(false);
                DistanceToNearestArtifact = 0;
            }
        }

        private void OnDestroy()
        {
            DistanceChanged?.RemoveAllListeners();
            ArtifactsDetected?.RemoveAllListeners();
        }
    }
}