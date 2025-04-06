using System.Globalization;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class OxygenUI : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI OxygenCount { get; private set; }

        public void OnOxygenCountChanged(float amount)
        {
            OxygenCount.text = amount.ToString("F1",CultureInfo.InvariantCulture);
        }
    }
}
