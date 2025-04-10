using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Detector : MonoBehaviour
    {
        public UnityEvent DistanceChanged;
        public UnityEvent ArtifactsDetected;
        [SerializeField] private float distanceToNearestArtifact;

        [FormerlySerializedAs("artifactDetected")] [SerializeField]
        private bool _artifactDetected;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;

        [field: SerializeField]
        public bool RevealingMode { get; private set; }
        
        public bool ArtifactDetected
        {
            get => _artifactDetected;
            private set
            {
                _artifactDetected = value;
                ArtifactsDetected?.Invoke();
            }
        }

        [field: SerializeField]
        public Artifact NearestArtifact { get; private set; }

        [field: SerializeField]
        public float Range { get; private set; }

        [field: SerializeField]
        public float RevealingRange { get; private set; }
        
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

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
            _collider2D = GetComponent<Collider2D>();
            _collider2D.enabled = false;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RevealingMode = !RevealingMode;
                _spriteRenderer.enabled = RevealingMode;
                _collider2D.enabled = RevealingMode;
            }
            
            if (RevealingMode)
            {
                var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0f;

                var direction = (mouseWorldPosition - transform.parent.position).normalized;
                transform.position = transform.parent.position + direction * RevealingRange;
            }
            else
            {
                transform.position = transform.parent.position;
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
                DistanceToNearestArtifact = Vector3.Distance(transform.position, NearestArtifact.transform.position);
                ArtifactDetected = true;
            }
            else
            {
                DistanceToNearestArtifact = 0;
                ArtifactDetected = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Artifact>(out var artifact))
            {
                artifact.Reveal();
            }
        }

        private void OnDestroy()
        {
            DistanceChanged?.RemoveAllListeners();
            ArtifactsDetected?.RemoveAllListeners();
        }
    }
}