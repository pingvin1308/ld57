using System.Linq;
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

        public void StartDialog(DialogNode startNode)
        {
            _currentNode = startNode;
            ShowCurrentNode();
        }

        private void ShowCurrentNode()
        {
            ClearChoices();

            var speaker = FindSpeaker(_currentNode.SpeakerId);
            if (speaker != null)
            {
                _activeBubble = Instantiate(speechBubblePrefab, canvas.transform);
                var ui = _activeBubble.GetComponent<SpeechBubbleUI>();
                ui.Initialize(speaker, _currentNode.Text, Mathf.Max(2f, _currentNode.Text.Length * 0.3f));
            }

            if (_currentNode.Choices != null && _currentNode.Choices.Count > 0)
            {
                foreach (var choice in _currentNode.Choices)
                {
                    // var btn = Instantiate(choiceButtonPrefab, choicesParent);
                    // btn.GetComponentInChildren<TMPro.TMP_Text>().text = choice.Text;
                    //
                    // btn.GetComponent<Button>().onClick.AddListener(() =>
                    // {
                    //     if (_activeBubble != null) Destroy(_activeBubble);
                    //     _currentNode = choice.NextNode;
                    //     ShowCurrentNode();
                    // });
                }
            }
        }

        private Transform FindSpeaker(SpeakerId id)
        {
            return FindObjectsOfType<DialogSpeaker>()
                .FirstOrDefault(s => s.SpeakerId == id)?.transform;
        }

        private void ClearChoices()
        {
            foreach (Transform child in choicesParent)
                Destroy(child.gameObject);
        }
    }
}