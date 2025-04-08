using UnityEngine;

namespace Game.Scripts.UI
{
    public class DetectorUI : MonoBehaviour
    {
        [field: SerializeField]
        public SignalUI SignalUI { get; private set; }

        [field: SerializeField]
        public Detector Detector { get; private set; }

        private void OnEnable()
        {
            Detector.DistanceChanged.AddListener(OnDistanceChanged);
            Detector.ArtifactsDetected.AddListener(OnArtifactsDetected);
        }

        private void OnDisable()
        {
            Detector.DistanceChanged.RemoveListener(OnDistanceChanged);
            Detector.ArtifactsDetected.RemoveListener(OnArtifactsDetected);
        }

        private void OnArtifactsDetected()
        {
            if (!Detector.ArtifactDetected)
            {
                SignalUI.SetValue(0);
            }
        }

        private void OnDistanceChanged()
        {
            if (!Detector.ArtifactDetected)
            {
                return;
            }

            var scale = SignalUI.MaxValue / Detector.Range;
            var scaledValue = Mathf.Floor(Detector.DistanceToNearestArtifact) * scale;
            var signalValue = SignalUI.MaxValue - Mathf.Clamp(scaledValue, SignalUI.MinValue, SignalUI.MaxValue);
            SignalUI.SetValue((int)signalValue);
        }
    }
}