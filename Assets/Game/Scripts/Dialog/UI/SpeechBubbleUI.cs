using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.Scripts.Dialog.UI
{
    public class SpeechBubbleUI : MonoBehaviour, IPointerClickHandler
    {
        [FormerlySerializedAs("textField")] [SerializeField]
        private TextMeshProUGUI _textField;
        private AudioSource _audioSource;

        private Transform _target;
        private bool _isTyping;
        private bool _skipRequested;
        private Coroutine _typingCoroutine;
        private SpeechBubblePanelUI _panelUI;
        private Vector3 _offset;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public IEnumerator Initialize(Vector3 speakerOffset, Transform target, string text, float duration)
        {
            _offset = speakerOffset;
            _target = target;
            _textField.text = text;
            _textField.maxVisibleCharacters = 0;
            _typingCoroutine = StartCoroutine(TypeTextCoroutine(text, duration));
            yield return AutoPosition();
        }

        private IEnumerator TypeTextCoroutine(string text, float duration)
        {
            _isTyping = true;
            _skipRequested = false;

            var specialChars = new HashSet<char> { '.', ',', '!', '?' };

            foreach (var c in text)
            {
                _audioSource.Play();
                _textField.maxVisibleCharacters++;
                if (_skipRequested)
                {
                    duration = 0.01f;
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