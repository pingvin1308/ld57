using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Dialog.UI
{
    public class SpeechBubbleUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI textField;

        private Transform _target;
        private bool _isTyping;
        private bool _skipRequested;
        private Coroutine _typingCoroutine;
        private SpeechBubblePanelUI _panelUI;
        private Vector3 _offset;

        public IEnumerator Initialize(Vector3 speakerOffset, Transform target, string text, float duration)
        {
            _offset = speakerOffset;
            _target = target;
            textField.text = text;
            _typingCoroutine = StartCoroutine(TypeTextCoroutine(text, duration));
            yield return AutoPosition();
        }

        private IEnumerator TypeTextCoroutine(string text, float duration)
        {
            _isTyping = true;
            _skipRequested = false;
            textField.text = "";
            
            var specialChars = new HashSet<char> { '.', ',', '!', '?' };

            foreach (var c in text)
            {
                textField.text += c;

                if (_skipRequested)
                {
                    duration = 0.01f;
                    // textField.text = text;
                    // break;
                }
                
                if (specialChars.Contains(c))
                {
                    yield return new WaitForSecondsRealtime(duration * 8);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(duration);
                }
            }

            yield return new WaitForSecondsRealtime(text.Length * duration); 
            
            _isTyping = false;
            Destroy(gameObject);
        }

        private IEnumerator AutoPosition()
        {
            yield return null;
            while (_isTyping)
            {
                if (_target != null)
                {
                    transform.position = _target.parent.position + _offset;
                }
        
                yield return null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isTyping)
            {
                _skipRequested = true;
            }
        }
    }
}