using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Inventory
{
    public class ArtifactsUI : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI ArtifactsCount { get; private set; }

        [field: SerializeField]
        public ArtifactItemUI ArtifactItemUIPrefab { get; private set; }
        
        [field: SerializeField]
        public List<ArtifactItemUI> Artifacts { get; private set; }     
        
        [field: SerializeField]
        public Scripts.Inventory Inventory { get; private set; }

        private void OnEnable()
        {
            Inventory.OnInventoryChanged.AddListener(UpdateUI);
            UpdateUI();
        }

        private void OnDisable()
        {
            Inventory.OnInventoryChanged.RemoveAllListeners();
        }

        private void UpdateUI()
        {
            foreach (var artifact in Artifacts)
            {
                Destroy(artifact.gameObject);
            }
            Artifacts.Clear();
            
            foreach (var artifact in Inventory.CollectedArtifacts)
            {
                var artifactGameObject = Instantiate(ArtifactItemUIPrefab, transform);
                artifactGameObject.Init(artifact);
                artifactGameObject.ArtifactDroped.AddListener(Inventory.DropArtifact);
                Artifacts.Add(artifactGameObject);
                ArtifactsCount.text = Artifacts.Count.ToString();
            }            
        }
    }
}