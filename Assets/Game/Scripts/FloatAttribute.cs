using System;
using Sirenix.OdinInspector;

namespace Game.Scripts
{
    [Serializable]
    [InlineProperty]
    public class FloatAttribute : Attribute<float>
    {
        public float Current => BaseValue + Modifier;
    }
    
    [Serializable]
    [InlineProperty]
    public class IntAttribute : Attribute<int>
    {
        public int Current => BaseValue + Modifier;
    }
    
}