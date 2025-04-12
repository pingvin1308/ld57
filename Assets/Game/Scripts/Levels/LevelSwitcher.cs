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

        [field: SerializeField]
        public UpLevelTrigger UpLevelTrigger { get; private set; }
        
        [field: SerializeField]
        public Player Player { get; private set; }
        
        private void Awake()
        {
            CurrentLevel.Enable();
        }

        public void BackToHub()
        {
            foreach (var level in _levelCache)
            {
                Destroy(level.Value.gameObject);
            }
            
            CurrentLevel.Disable();
            Player.transform.position = UpLevelTrigger.transform.position + new Vector3(0, -3);

            CurrentLevel = ExpeditionHub;
            CurrentLevel.Enable();
            _levelCache.Clear();
        }

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
                Debug.Log($"Generate level: {nextLevelNumber}");
                var createdLevel = LevelGenerator.Generate(nextLevelNumber);
                _levelCache.Add(nextLevelNumber, createdLevel);
                return createdLevel;
            }

            return ExpeditionHub;
        }
    }
}