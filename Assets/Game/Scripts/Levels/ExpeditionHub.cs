using UnityEngine;

namespace Game.Scripts.Levels
{
    public class ExpeditionHub : LevelBase
    {
        private void Update()
        {
            if (Player != null)
            {
                var restoreRate = (Player.Oxygen.MaxVolume.Current / 100) * Player.Oxygen.RestoreRatePercent;
                Player.Oxygen.Restore(restoreRate * Time.deltaTime);
            }
        }
    }
}