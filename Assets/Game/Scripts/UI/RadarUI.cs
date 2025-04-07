using UnityEngine;

namespace Game.Scripts.UI
{
    public class RadarUI : MonoBehaviour
    {
        [field: SerializeField]
        public bool ArtifactDetected { get; private set; }

        [field: SerializeField]
        public SignalUI SignalUI { get; private set; }

        public void OnArtifactsDetected(bool detected)
        {
            ArtifactDetected = detected;

            if (!detected)
            {
                SignalUI.SetValue(0);
            }
        }

        public void OnDistanceChanged(float range, float distance)
        {
            if (!ArtifactDetected)
            {
                return;
            }

            var scale = SignalUI.MaxValue / range;
            var scaledValue = distance * scale;
            var signalValue = SignalUI.MaxValue - Mathf.Clamp(scaledValue, SignalUI.MinValue, SignalUI.MaxValue);
            SignalUI.SetValue((int)signalValue);
        }
    }
}