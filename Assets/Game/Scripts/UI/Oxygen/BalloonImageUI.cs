using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    [RequireComponent(typeof(Image))]
    public class BalloonImageUI : MonoBehaviour
    {
        private Image _image;

        [SerializeField] private Sprite[] _images;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetUpgradeLevel(int index)
        {
            _image.sprite = _images[index];
        }
    }
}