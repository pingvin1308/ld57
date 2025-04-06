using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class ArtifactsUI : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI ArtifactsCount { get; private set; }

        public void OnArtifactsCountChanged(int amount)
        {
            ArtifactsCount.text = amount.ToString();
        }
    }
}