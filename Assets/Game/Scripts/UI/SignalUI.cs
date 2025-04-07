using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(Image))]
    public class SignalUI : MonoBehaviour
    {
        private Image _currentSignalImage;

        [SerializeField] 
        private Sprite[] sprites;
        
        [field: SerializeField]
        public int CurrentValue { get; private set; }
        
        
        [field: SerializeField]
        public int MinValue { get; private set; } = 0;
        
        [field: SerializeField]
        public int MaxValue { get; private set; } = 10;
        
        private void Awake()
        {
            sprites = Resources.LoadAll<Sprite>("Detector/detector"); // имя файла без расширения
            _currentSignalImage = GetComponent<Image>();
            _currentSignalImage.sprite = sprites[0];
        }

        public void SetValue(int value)
        {
            CurrentValue = Math.Min(value, MaxValue);
            _currentSignalImage.sprite = sprites[CurrentValue];
        }
    }
}
