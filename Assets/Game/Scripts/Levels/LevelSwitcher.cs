using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Levels
{
    public class LevelSwitcher : MonoBehaviour
    {
        private readonly Dictionary<int, LevelData> _levelCache = new();

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
            CurrentLevel.Enable(Player);
        }

        public void BackToHub()
        {
            // foreach (var level in _levelCache)
            // {
            //     Destroy(level.Value.gameObject);
            // }

            Destroy(CurrentLevel.gameObject);
            // CurrentLevel.Disable();
            Player.transform.position = UpLevelTrigger.transform.position + new Vector3(0, -3);
            CurrentLevel = ExpeditionHub;
            CurrentLevel.Enable(Player);
            _levelCache.Clear();
        }

        public void SwitchLevel(LevelDirection direction)
        {
            var nextLevelNumber = CurrentLevel.LevelNumber + (int)direction;
            if (nextLevelNumber == ExpeditionHub.LevelNumber)
            {
                BackToHub();
                return;
            }

            var nextLevel = GetNextLevel(nextLevelNumber);
            nextLevel.Enable(Player);
            if (CurrentLevel != ExpeditionHub)
            {
                Destroy(CurrentLevel.gameObject);
            }
            else
            {
                ExpeditionHub.Disable();
            }

            // CurrentLevel.Disable();
            CurrentLevel = nextLevel;
        }

        private LevelBase GetNextLevel(int nextLevelNumber)
        {
            if (_levelCache.TryGetValue(nextLevelNumber, out var levelData))
            {
                var level = LevelGenerator.Generate(nextLevelNumber, levelData);
                Debug.Log($"Restore level: {nextLevelNumber}");
                return level;
            }

            Debug.Log($"Generate level: {nextLevelNumber}");
            var createdLevel = LevelGenerator.Generate(nextLevelNumber);
            _levelCache.Add(nextLevelNumber, createdLevel.Data);
            return createdLevel;
        }
    }
}