using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class MoneyUI : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI Money { get; private set; }

        public void OnMoneyChanged(int amount)
        {
            Money.text = amount.ToString(); 
        }
    }
}