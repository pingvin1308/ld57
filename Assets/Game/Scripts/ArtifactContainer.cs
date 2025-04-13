using System;
using Game.Scripts.Artifacts;
using UnityEngine;

namespace Game.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ArtifactContainer : MonoBehaviour
    {
        [SerializeField] private InteractionArea _interactionArea;
        private SpriteRenderer _spriteRenderer;

        [field: SerializeField]
        public ArtifactData Artifact { get; private set; }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _interactionArea.PlayerEntered.AddListener(OnPlayerEntered);
            _interactionArea.PlayerExited.AddListener(OnPlayerExited);
            _interactionArea.ArtifactPlaced.AddListener(OnPArtifactPlaced);
        }

        private void OnDisable()
        {
            _interactionArea.PlayerEntered.RemoveListener(OnPlayerEntered);
            _interactionArea.PlayerExited.RemoveListener(OnPlayerExited);
            _interactionArea.ArtifactPlaced.AddListener(OnPArtifactPlaced);
        }

        private void OnPlayerEntered()
        {
            Debug.Log("ArtifactContainer: Player entered");
            _spriteRenderer.material.SetFloat("_Thickness", 1.0f);
        }

        private void OnPlayerExited()
        {
            Debug.Log("ArtifactContainer: Player exited");
            _spriteRenderer.material.SetFloat("_Thickness", 0);
        }

        private void OnPArtifactPlaced(ArtifactData artifactData)
        {
            Debug.Log("ArtifactContainer: Artifact placed");
            Artifact = artifactData;
        }
    }
}