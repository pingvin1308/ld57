using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(Image))]
    public class SignalUI : MonoBehaviour
    {
        private Image _currentSignalImage;

        [SerializeField]
        private Sprite _brokenSprite; 
        
        [SerializeField] 
        private Sprite[] _sprites;
        
        [field: SerializeField]
        public int CurrentValue { get; private set; }
        
        [field: SerializeField]
        public int MinValue { get; private set; } = 0;
        
        [field: SerializeField]
        public int MaxValue { get; private set; } = 10;
        
        private void Awake()
        {
            _currentSignalImage = GetComponent<Image>();
            _currentSignalImage.sprite = _brokenSprite;
        }

        public void SetValue(int value)
        {
            CurrentValue = Math.Min(value, MaxValue);
            _currentSignalImage.sprite = _sprites[CurrentValue];
        }
    }
}
