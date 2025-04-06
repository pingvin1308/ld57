using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// AKA СИДОРОВИЧ
    /// </summary>
    public class Visitor : MonoBehaviour
    {
    }

    public class Order
    {
        public IReadOnlyCollection<ArtifactData> Goal { get; set; }
        
        /// <summary>
        /// Money
        /// </summary>
        public int Reward { get; set; }
        
        public void Complete(IReadOnlyCollection<ArtifactData> artifacts)
        {
            
        }
    }
}