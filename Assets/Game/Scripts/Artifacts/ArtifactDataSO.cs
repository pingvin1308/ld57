using UnityEngine;

namespace Game.Scripts.Artifacts
{
    [CreateAssetMenu(menuName = "Artifacts/ArtifactData")]
    public class ArtifactDataSO : ScriptableObject
    {
        [field: SerializeField]
        public ArtifactId ArtifactId { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; private set; }

        [field: SerializeField]
        public int BasePrice { get; private set; }

        [field: SerializeField]
        public ArtifactRarity Rarity { get; private set; }

        [field: SerializeField]
        public ArtifactType Type { get; private set; }

        [field: SerializeField]
        public LevelType LevelType { get; private set; }

        [field: SerializeField]
        public AcquisitionMethod AcquisitionMethod { get; private set; }

        [field: SerializeField]
        public int StartingLevel { get; private set; }
    }
}