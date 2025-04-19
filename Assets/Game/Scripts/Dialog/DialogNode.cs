using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Dialog
{
    [CreateAssetMenu(menuName = "Dialog/Node")]
    public class DialogNode : ScriptableObject
    {
        [field: SerializeField]
        public SpeakerId SpeakerId { get; set; }

        [TextArea(2, 4)]
        [field: SerializeField]
        public string Text { get; set; }

        [field: SerializeField]
        public bool RequiresAttention { get; private set; }

        [field: SerializeField]
        public List<DialogChoice> Choices { get; private set; }

        [field: SerializeField]
        public DialogNode NextNode { get; private set; }
        
        public void AddChoice(DialogChoice choice)
        {
            if (Choices == null)
            {
                Choices = new List<DialogChoice>();
            }

            Choices.Add(choice);
        }
    }

    public enum SpeakerId
    {
        Harold,
        Phil,
        CommonHunter
    }
}