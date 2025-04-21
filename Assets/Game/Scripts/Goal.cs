using System;

namespace Game.Scripts.Dialog.Characters
{
    [Serializable]
    public class Goal
    {
        public string Text { get; private set; }
        public Func<bool> Condition { get; private set; }
        public bool Achieved => Condition?.Invoke() ?? true;

        public Goal(string text, Func<bool> condition)
        {
            Text = text;
            Condition = condition;
        }
    }
}