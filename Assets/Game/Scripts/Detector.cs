using Game.Scripts.Artifacts;
using UnityEngine;

namespace Game.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Detector : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;

        [field: SerializeField]
        public ScanArea ScanArea { get; private set; }

        [field: SerializeField]
        public bool RevealingMode { get; private set; }

        [field: SerializeField]
        public Artifact NearestArtifact { get; private set; }

        [field: SerializeField]
        public float Range { get; private set; }

        [field: SerializeField]
        public float RevealingRange { get; private set; }
        


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
     
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Artifact>(out var artifact) && artifact.Data.ArtifactId != ArtifactId.GreedyCoin)
            {
                artifact.Reveal();
            }
        }

        private void OnDestroy()
        {
        }
    }
}