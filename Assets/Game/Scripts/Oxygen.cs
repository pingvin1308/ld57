using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class Oxygen : MonoBehaviour
    {
        public UnityEvent VolumeChanged;

        [FormerlySerializedAs("volume")] [SerializeField]
        private float _volume;

        [field: SerializeField]
        public int UpgradeLevel { get; private set; }
        
        public float Volume
        {
            get => _volume;
            set
            {
                if (!Mathf.Approximately(_volume, value))
                {
                    _volume = Mathf.Clamp(value, 0, MaxVolume.Current);
                    VolumeChanged?.Invoke();
                }
            }
        }

        [field: SerializeField]
        public FloatAttribute MaxVolume { get; private set; }

        public void Use(float amount)
        {
            Volume -= amount;
        }

        public void ApplyUpgrade(float upgrade, int upgradeLevel)
        {
            UpgradeLevel = upgradeLevel;
            MaxVolume.Apply(upgrade);
        }

        public void Restore()
        {
            Volume = MaxVolume.Current;
        }

        public void OnDestroy()
        {
            VolumeChanged?.RemoveAllListeners();
        }
    }
}