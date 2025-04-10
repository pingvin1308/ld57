using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Artifacts
{
    [CreateAssetMenu(menuName = "Artifacts/ArtifactsDatabase")]
    public class ArtifactsDatabase : ScriptableObject
    {
        [field: SerializeField]
        public List<ArtifactData> AllArtifacts { get; private set; }

        private void Awake()
        {
            LoadAllArtifacts();
        }

        private void LoadAllArtifacts()
        {
            var loaded = Resources.LoadAll<ArtifactData>("Artifacts");
            AllArtifacts = new List<ArtifactData>(loaded);
            Debug.Log($"Загружено {AllArtifacts.Count} артефактов.");
        }
        
        public ArtifactData GetByID(ArtifactId id)
        {
            return AllArtifacts.Find(x => x.ArtifactId == id);
        }
    }
}