using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class GoalUI : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI GoalValue { get; private set; }

        public void OnGoalValueChanged(int goalValue)
        {
            GoalValue.text = goalValue.ToString();
        }
    }
}