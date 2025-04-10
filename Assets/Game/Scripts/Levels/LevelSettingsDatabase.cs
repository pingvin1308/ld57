using System;
using System.Linq;
using Game.Scripts.Artifacts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Levels
{
    [CreateAssetMenu(menuName = "Levels/LevelSettingsDatabase")]
    public class LevelSettingsDatabase : ScriptableObject
    {
        [field: SerializeField]
        public LevelSettings[] LevelSettings { get; private set; }

        public ArtifactId GetRandomArtifactId(int levelNumber)
        {
            var levelSettings = LevelSettings.FirstOrDefault(x => x.LevelNumber == Math.Abs(levelNumber));
            if (levelSettings == null)
            {
                var allArtifacts = Enum.GetValues(typeof(ArtifactId));
                return (ArtifactId)allArtifacts.GetValue(Random.Range(0, allArtifacts.Length));
            }

            return levelSettings.ArtifactIds[Random.Range(0, levelSettings.ArtifactIds.Length)];
        }
    }
}