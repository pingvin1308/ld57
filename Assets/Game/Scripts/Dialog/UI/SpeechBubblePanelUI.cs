using System.Collections;
using UnityEngine;

namespace Game.Scripts.Dialog.UI
{
    public class SpeechBubblePanelUI: MonoBehaviour
    {
        [SerializeField]
        private SpeechBubbleUI _bubbleUI;
        
        public IEnumerator Show(Vector3 speakerOffset, Transform target, string text, float duration)
        {
            yield return _bubbleUI.Initialize(speakerOffset, target, text, duration);
        }
    }
}