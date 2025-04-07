using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Artifacts/ArtifactData")]
    public class ArtifactData : ScriptableObject
    {
        [field: SerializeField]
        public ArtifactId ArtifactId { get; set; }
        
        [field: SerializeField]
        public Sprite Sprite { get; private set; }
        
        [field: SerializeField]
        public int Value { get; private set; }
        
        [field: SerializeField]
        public Rarity Rarity { get; private set; }
    }
    
    
    public enum ArtifactId
    {
        MovementMirror = 0,
        ErosionMud,
        GreedyCoin,
        LightWeight,
        Sneakers,
        SlippyBanana,
        StinkyCheese
        
    }
    
    
}