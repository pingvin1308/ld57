using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Artifacts;

namespace Game.Scripts
{
    [Serializable]
    public class OrderData
    {
        /// <summary>
        /// Sum of artifacts value
        /// </summary>
        public int InitialGoal { get; }

        /// <summary>
        /// Sum of artifacts value
        /// </summary>
        public int Goal { get; private set; }

        /// <summary>
        /// Money
        /// </summary>
        public int Reward { get; private set; }

        public OrderData(int goal, int reward)
        {
            InitialGoal = goal;
            Goal = goal;
            Reward = reward;
        }

        public bool TryComplete(IReadOnlyCollection<ArtifactDataSO> artifacts)
        {
            Goal -= artifacts.Sum(x => x.BasePrice);
            return Goal <= 0;
        }
    }
}