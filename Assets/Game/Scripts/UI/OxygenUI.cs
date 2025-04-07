using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class OxygenUI : MonoBehaviour
    {
        [field: SerializeField]
        public Slider OxygenVolume { get; private set; }

        private float _maxVolume = 100;

        private void Awake()
        {
            OxygenVolume.maxValue = _maxVolume;
            OxygenVolume.value = _maxVolume;
        }

        public void OnOxygenCountChanged(float amount)
        {
            OxygenVolume.value = amount;
        }
    }
}
