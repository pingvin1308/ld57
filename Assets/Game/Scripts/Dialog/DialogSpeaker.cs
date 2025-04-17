using UnityEngine;

namespace Game.Scripts.Dialog
{
    public class DialogSpeaker : MonoBehaviour
    {
        [field: SerializeField]
        public SpeakerId SpeakerId { get; private set; }

        [field: SerializeField]
        public string DisplayName { get; private set; }
    }
}