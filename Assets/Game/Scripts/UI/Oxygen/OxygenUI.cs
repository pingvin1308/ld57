using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class OxygenUI : MonoBehaviour
    {
        private Vector3 _startAnchoredPosition;
        private RectTransform _rectTransform;
        
        [field: SerializeField]
        public BalloonImageUI BalloonImage { get; private set; }
        
        [field: SerializeField]
        public Slider OxygenVolume { get; private set; }

        [field: SerializeField]
        public Oxygen Oxygen { get; private set; }

        public void ResetPosition()
        {
            _rectTransform.anchoredPosition = _startAnchoredPosition;
        }
        
        public void OnOxygenCountChanged()
        {
            OxygenVolume.maxValue = Oxygen.MaxVolume.Current;
            OxygenVolume.value = Oxygen.Volume;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startAnchoredPosition = _rectTransform.anchoredPosition;
            OxygenVolume.maxValue = Oxygen.MaxVolume.Current;
            OxygenVolume.value = Oxygen.Volume;
        }

        private void OnEnable()
        {
            Oxygen.VolumeChanged.AddListener(OnOxygenCountChanged);
        }

        private void OnDisable()
        {
            Oxygen.VolumeChanged.RemoveListener(OnOxygenCountChanged);
        }
    }
}