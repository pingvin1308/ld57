using System;
using UnityEngine;

namespace Game.Scripts.Artifacts
{
    [Serializable]
    public class ArtifactData
    {
        public ArtifactId ArtifactId { get; private set; }

        public Sprite Sprite { get; private set; }

        public int BasePrice { get; private set; }

        public ArtifactRarity Rarity { get; private set; }

        public ArtifactType Type { get; private set; }

        public LevelType LevelType { get; private set; }

        public AcquisitionMethod AcquisitionMethod { get; private set; }

        public int StartingLevel { get; private set; }

        public int SpawnedAtLevel { get; set; }
        
        public Vector3 Position { get; set; }
        
        public bool IsRevealed { get; set; }

        public ArtifactData(ArtifactDataSO data)
        {
            ArtifactId = data.ArtifactId;
            Sprite = data.Sprite;
            BasePrice = data.BasePrice;
            Rarity = data.Rarity;
            Type = data.Type;
            LevelType = data.LevelType;
            AcquisitionMethod = data.AcquisitionMethod;
            StartingLevel = data.StartingLevel;
        }
        
        public int GetFinalPrice()
        {
            var depthMultiplier = 1 + (Math.Abs(SpawnedAtLevel) / 3.0f) * 0.2f;
            var levelFactor = LevelType switch
            {
                LevelType.Common => 1f,
                LevelType.Trapped => 1.5f,
                LevelType.Unique => 2f,
                _ => throw new ArgumentOutOfRangeException()
            };
            var rarityFactor = Rarity switch
            {
                ArtifactRarity.Common => 1,
                ArtifactRarity.Rare => 1.5f,
                ArtifactRarity.Unique => 2,
                _ => throw new ArgumentOutOfRangeException()
            };
            var accessibilityFactor = AcquisitionMethod switch
            {
                AcquisitionMethod.None => 1,
                AcquisitionMethod.Detector => 1.2f,
                AcquisitionMethod.Light => 1.3f,
                _ => throw new ArgumentOutOfRangeException()
            };

            var baseFactor = Type switch
            {
                ArtifactType.Currency => 1,
                ArtifactType.Negative => 0.7f,
                ArtifactType.Functional => 1.5f,
                ArtifactType.Cursed => 2,
                ArtifactType.Unique => 4,
                ArtifactType.Set => 1.2,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (int)Math.Floor(BasePrice
                                   * depthMultiplier
                                   * levelFactor
                                   * rarityFactor
                                   * accessibilityFactor
                                   * baseFactor);
        }
    }
}