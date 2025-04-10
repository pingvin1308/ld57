using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class OxygenUI : MonoBehaviour
    {
        [field: SerializeField]
        public Slider OxygenVolume { get; private set; }

        private float _maxVolume = 100;

        [field: SerializeField]
        public Oxygen Oxygen { get; private set; }
        
        private void Awake()
        {
            OxygenVolume.maxValue = _maxVolume;
            OxygenVolume.value = _maxVolume;
        }

        private void OnEnable()
        {
            Oxygen.VolumeChanged.AddListener(OnOxygenCountChanged);
        }

        private void OnDisable()
        {
            Oxygen.VolumeChanged.RemoveListener(OnOxygenCountChanged);
        }

        public void OnOxygenCountChanged()
        {
            OxygenVolume.maxValue = Oxygen.MaxVolume.Current;
            OxygenVolume.value = Oxygen.Volume;
        }
    }
}
