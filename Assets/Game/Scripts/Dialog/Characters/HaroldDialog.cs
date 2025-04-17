using UnityEngine;

namespace Game.Scripts.Dialog.Characters
{
    [RequireComponent(typeof(Collider2D))]
    public class HaroldDialog : MonoBehaviour
    {
        [field: SerializeField]
        public DialogRunner DialogRunner { get; private set; }

        private bool _isPlayerFinished = false;

        private string[] _tutorialDialog = new string[]
        {
            "Hello, I'm Harold. I have a quest for you!",
            "Hi again! Let's continue with another quest."
        };

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                // stages

                // random things by event
                var dialog = ScriptableObject.CreateInstance<DialogNode>();
                if (_isPlayerFinished)
                {
                    dialog.SpeakerId = SpeakerId.Harold;
                    dialog.Text = "Hello, I'm Phil. I have a quest for you!";
                }
                else
                {
                    dialog.SpeakerId = SpeakerId.Harold;
                    dialog.Text = "Hi again! Let's continue with another quest.";
                    _isPlayerFinished = true;
                }

                DialogRunner.StartDialog(dialog);
            }
        }
    }
}