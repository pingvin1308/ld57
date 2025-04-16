using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Artifacts
{
    [CreateAssetMenu(menuName = "Artifacts/ArtifactsDatabase")]
    public class ArtifactsDatabase : ScriptableObject
    {
        [field: SerializeField]
        public List<ArtifactDataSO> AllArtifacts { get; private set; }

        private void Awake()
        {
            LoadAllArtifacts();
        }

        private void LoadAllArtifacts()
        {
            var loaded = Resources.LoadAll<ArtifactDataSO>("Artifacts");
            AllArtifacts = new List<ArtifactDataSO>(loaded);
            Debug.Log($"Загружено {AllArtifacts.Count} артефактов.");
        }
        
        public ArtifactDataSO GetByID(ArtifactId id)
        {
            return AllArtifacts.Find(x => x.ArtifactId == id);
        }

        public ArtifactDataSO[] Get(int levelNumber, LevelType levelType)
        {
            return AllArtifacts
                .Where(x => x.StartingLevel <= levelNumber && x.LevelType == levelType)
                .ToArray();
        }
    }
}