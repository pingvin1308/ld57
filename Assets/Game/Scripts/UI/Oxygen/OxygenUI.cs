using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class OxygenUI : MonoBehaviour
    {
        private RectTransform _rectTransform;

        [field: SerializeField]
        public BalloonImageUI BalloonImage { get; private set; }

        [field: SerializeField]
        public Image OxygenBar { get; private set; }
        
        [field: SerializeField]
        public Slider OxygenVolume { get; private set; }

        [field: SerializeField]
        public Oxygen Oxygen { get; private set; }

        public void ResetPosition()
        {
            _rectTransform.anchoredPosition3D = Vector3.zero;
        }

        public void OnOxygenCountChanged()
        {
            OxygenVolume.maxValue = Oxygen.MaxVolume.Current;
            OxygenVolume.value = Oxygen.Volume;
            OxygenBar.color = Oxygen.HasLowVolume ? Color.red : Color.cyan;
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Awake()
        {
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