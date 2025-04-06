using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class RadarUI : MonoBehaviour
    {
        [field: SerializeField]
        public Slider Distance { get; private set; }
    }
}