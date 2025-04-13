using System;

namespace Game.Scripts.Artifacts
{
    public static class ArtifactDataExtensions
    {
        public static int GetFinalPrice(this ArtifactData artifactData)
        {
            var depthMultiplier = 1 + (Math.Abs(artifactData.SpawnedAtLevel) / 3.0f) * 0.2f;
            var levelFactor = artifactData.LevelType switch
            {
                LevelType.Common => 1f,
                LevelType.Trapped => 1.5f,
                LevelType.Unique => 2f,
                _ => throw new ArgumentOutOfRangeException()
            };
            var rarityFactor = artifactData.Rarity switch
            {
                ArtifactRarity.Common => 1,
                ArtifactRarity.Rare => 1.5f,
                ArtifactRarity.Unique => 2,
                _ => throw new ArgumentOutOfRangeException()
            };
            var accessibilityFactor = artifactData.AcquisitionMethod switch
            {
                AcquisitionMethod.None => 1,
                AcquisitionMethod.Detector => 1.2f,
                AcquisitionMethod.Light => 1.3f,
                _ => throw new ArgumentOutOfRangeException()
            };

            var baseFactor = artifactData.Type switch
            {
                ArtifactType.Currency => 1,
                ArtifactType.Negative => 0.7f,
                ArtifactType.Functional => 1.5f,
                ArtifactType.Cursed => 2,
                ArtifactType.Unique => 4,
                ArtifactType.Set => 1.2,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (int)Math.Floor(artifactData.BasePrice
                                   * depthMultiplier
                                   * levelFactor
                                   * rarityFactor
                                   * accessibilityFactor
                                   * baseFactor);
        }
    }
}