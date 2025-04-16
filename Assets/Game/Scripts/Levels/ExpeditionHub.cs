using Game.Scripts.Artifacts;
using UnityEngine;

namespace Game.Scripts.Levels
{
    public class ExpeditionHub : LevelBase
    {
        public ExpeditionHub()
        {
            Init(new LevelData(0, 0, LevelType.Common, 0));
        }

        private void Update()
        {
            if (Player != null)
            {
                var restoreRate = (Player.Oxygen.MaxVolume.Current / 100) * Player.Oxygen.RestoreRatePercent;
                Player.Oxygen.Restore(restoreRate * Time.deltaTime);
            }
        }

        public override void Disable()
        {
            gameObject.SetActive(false);
            base.Disable();
        }
    }
}