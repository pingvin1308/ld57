using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class Oxygen : MonoBehaviour
    {
        public UnityEvent<float> VolumeChanged;

        [FormerlySerializedAs("volume")] [SerializeField]
        private float _volume;

        public float Volume
        {
            get => _volume;
            set
            {
                if (!Mathf.Approximately(_volume, value))
                {
                    _volume = Mathf.Clamp(value, 0, MaxVolume);
                    VolumeChanged?.Invoke(_volume);
                }
            }
        }

        [field: SerializeField]
        public float Upgrade { get; private set; }
        
        [field: SerializeField]
        public float MaxVolume { get; private set; }

        public void Use(float amount)
        {
            float reduced = amount - Upgrade;
            float actualConsumption = Mathf.Max(reduced, amount * 0.1f);
            Volume -= actualConsumption;
        }

        public void ApplyUpgrade(float upgrade)
        {
            Upgrade += upgrade;
        }

        public void Restore()
        {
            Volume = MaxVolume;
        }
    }
}