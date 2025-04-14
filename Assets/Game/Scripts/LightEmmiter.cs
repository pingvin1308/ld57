using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Scripts
{
    [RequireComponent(typeof(Light2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class LightEmitter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Artifact>(out var artifact) &&
                artifact.Data.ArtifactId == ArtifactId.GreedyCoin)
            {
                artifact.Reveal();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Artifact>(out var artifact) &&
                artifact.Data.ArtifactId == ArtifactId.GreedyCoin)
            {
                artifact.Hide();
            }
        }
    }
}