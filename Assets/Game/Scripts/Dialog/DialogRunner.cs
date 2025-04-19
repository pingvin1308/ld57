using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Dialog.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Dialog
{
    public class DialogRunner : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject speechBubblePrefab;
        [SerializeField] private GameObject choiceButtonPrefab;
        [SerializeField] private Transform choicesParent;

        private DialogNode _currentNode;
        private GameObject _activeBubble;
        
        [field: SerializeField]
        public float TextSpeed { get; private set; } = 0.05f;

        public void StartDialog(DialogNode startNode)
        {
            _currentNode = startNode;
            // ShowCurrentNode();
        }
        private bool _awaitingInput;
        private bool _dialogFinished;

        public IEnumerator RunDialogSequence(List<DialogNode> nodes)
        {
            foreach (var node in nodes)
            {
                yield return ShowNodeCoroutine(node);
            }

            _dialogFinished = true;
        }

        private IEnumerator ShowNodeCoroutine(DialogNode node)
        {
            _currentNode = node;

            ClearChoices();
            var speaker = FindSpeaker(node.SpeakerId); 
            if (speaker != null)
            {
                _activeBubble = Instantiate(speechBubblePrefab, speaker.transform.parent.position + speaker.Offset, Quaternion.identity, canvas.transform);
                var ui = _activeBubble.GetComponent<SpeechBubblePanelUI>();
                // var duration = Mathf.Max(2f, node.Text.Length * 0.1f);
                yield return ui.Show(speaker.Offset, speaker.transform, node.Text, TextSpeed);
            }

            // yield return new WaitForSeconds(node.Text.Length * 0.05f);
            
            // _awaitingInput = true;
            //
            // while (_awaitingInput)
            // {
            //     if (Input.GetKeyDown(KeyCode.Space))
            //         _awaitingInput = false;
            //
            //     yield return null;
            // }

            if (_activeBubble != null)
                Destroy(_activeBubble);
        }
        

        private void ShowCurrentNode()
        {
            ClearChoices();

            var speaker = FindSpeaker(_currentNode.SpeakerId);
            if (speaker != null)
            {
                _activeBubble = Instantiate(speechBubblePrefab, speaker.transform.position, Quaternion.identity, canvas.transform);
                var ui = _activeBubble.GetComponent<SpeechBubbleUI>();
                // ui.Initialize(speaker, _currentNode.Text, Mathf.Max(2f, _currentNode.Text.Length * 0.3f));
            }
        }

        private DialogSpeaker FindSpeaker(SpeakerId id)
        {
            return FindObjectsOfType<DialogSpeaker>()?.FirstOrDefault(s => s.SpeakerId == id);
        }

        private void ClearChoices()
        {
            foreach (Transform child in choicesParent)
                Destroy(child.gameObject);
        }
    }
}