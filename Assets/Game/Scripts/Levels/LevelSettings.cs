using Game.Scripts.Artifacts;
using UnityEngine;

namespace Game.Scripts.Levels
{
    [CreateAssetMenu(menuName = "Levels/LevelSettings")]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField]
        public int LevelNumber { get; private set; }

        [field: SerializeField]
        public ArtifactId[] ArtifactIds { get; private set; }

        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }
    }
}