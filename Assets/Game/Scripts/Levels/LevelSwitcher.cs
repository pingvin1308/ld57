using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Levels
{
    public class LevelSwitcher : MonoBehaviour
    {
        private readonly Dictionary<int, LevelBase> _levelCache = new();
    
        [field: SerializeField]
        public LevelGenerator LevelGenerator { get; private set; }
    
        [field: SerializeField]
        public LevelBase CurrentLevel { get; private set; }

        
        [field: SerializeField]
        public ExpeditionHub ExpeditionHub { get; private set; }
        
        public void SwitchLevel(LevelDirection direction)
        {
            var nextLevel = GetNextLevel(direction);

            if (nextLevel == ExpeditionHub)
            {
                // reset levels
                foreach (var level in _levelCache)
                {
                    Destroy(level.Value.gameObject);
                }
                _levelCache.Clear();
            }
            
            CurrentLevel.Disable();
            CurrentLevel = nextLevel;
            CurrentLevel.Enable();
        }

        private LevelBase GetNextLevel(LevelDirection direction)
        {
            var nextLevelNumber = CurrentLevel.LevelNumber + (int)direction;

            if (_levelCache.TryGetValue(nextLevelNumber, out var level))
            {
                return level;
            }

            if (direction == LevelDirection.Down)
            {
                var createdLevel = LevelGenerator.Generate(nextLevelNumber);
                _levelCache.Add(nextLevelNumber, createdLevel);
                return createdLevel;
            }

            return ExpeditionHub;
        }
    }
}