using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Dialog
{
    public class SpeechBubbleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;
        private Transform _target;
        [SerializeField] private Vector3 _offset;

        public void Initialize(Transform target, string text, float duration)
        {
            _target = target;
            textField.text = text;
            StartCoroutine(DestroyAfter(duration));
        }

        private void Update()
        {
            if (_target != null)
            {
                // Camera.main.ScreenToWorldPoint();
                Debug.Log(_target.parent.position);
                transform.position = _target.parent.position + _offset; //Camera.main.WorldToScreenPoint();
            }
        }

        private IEnumerator DestroyAfter(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            Destroy(gameObject);
        }
    }
}