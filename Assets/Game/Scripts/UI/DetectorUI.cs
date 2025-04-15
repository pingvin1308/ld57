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
            Detector.ScanArea.DistanceChanged.AddListener(OnDistanceChanged);
            Detector.ScanArea.ArtifactsDetected.AddListener(OnArtifactsDetected);
        }

        private void OnDisable()
        {
            Detector.ScanArea.DistanceChanged.RemoveListener(OnDistanceChanged);
            Detector.ScanArea.ArtifactsDetected.RemoveListener(OnArtifactsDetected);
        }

        private void OnArtifactsDetected()
        {
            if (!Detector.ScanArea.ArtifactDetected)
            {
                SignalUI.SetValue(0);
            }
        }

        private void OnDistanceChanged()
        {
            if (!Detector.ScanArea.ArtifactDetected)
            {
                return;
            }

            var scale = SignalUI.MaxValue / Detector.Range;
            var scaledValue = Mathf.Floor(Detector.ScanArea.DistanceToNearestArtifact) * scale;
            var signalValue = SignalUI.MaxValue - Mathf.Clamp(scaledValue, SignalUI.MinValue, SignalUI.MaxValue);
            SignalUI.SetValue((int)signalValue);
        }
    }
}