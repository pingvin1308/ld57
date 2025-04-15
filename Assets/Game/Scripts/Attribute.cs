using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts
{
    [Serializable]
    [InlineProperty]
    public class Attribute<TValue>
    {
        [field: SerializeField]
        public TValue BaseValue { get; private set; }

        [field: SerializeField]
        public TValue Modifier { get; private set; }

        public void Reset()
        {
            Modifier = default;
        }

        public void Apply(TValue modified)
        {
            Modifier = modified;
        }
    }
}