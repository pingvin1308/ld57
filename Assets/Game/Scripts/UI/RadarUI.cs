using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class RadarUI : MonoBehaviour
    {
        [field: SerializeField]
        public Slider Distance { get; private set; }

        [field: SerializeField]
        public bool ArtifactDetected { get; private set; }
        
        public void OnArtifactsDetected(bool detected)
        {
            ArtifactDetected = detected;

            if (!detected)
            {
                Distance.value = 0;
            }
        }
        
        public void OnDistanceChanged(float range, float distance)
        {
            if (!ArtifactDetected)
            {
                return;
            }
            
            var scale = Distance.maxValue / range;
            var value = distance * scale;
            Distance.value = Distance.maxValue - Mathf.Clamp(value, Distance.minValue, Distance.maxValue);
        }
    }
}