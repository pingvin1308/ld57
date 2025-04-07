using System.Collections.Generic;
using Game.Scripts.UI.Inventory;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
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
        public ArtifactSpawner ArtifactSpawner { get; private set; }
        
        public void OnArtifactsCountChanged(Scripts.Inventory inventory)
        {
            foreach (var artifact in Artifacts)
            {
                if (artifact != null)
                {
                    Destroy(artifact.gameObject);
                }
            }
            Artifacts.Clear();
            
            foreach (var artifact in inventory.CollectedArtifacts)
            {
                var artifactGameObject = Instantiate(ArtifactItemUIPrefab, transform);
                artifactGameObject.Init(artifact);
                Artifacts.Add(artifactGameObject);
                ArtifactsCount.text = Artifacts.Count.ToString();
                    
                artifactGameObject.ArtifactDroped.AddListener(ArtifactSpawner.DropArtifact);
            }            
        }
    }
}