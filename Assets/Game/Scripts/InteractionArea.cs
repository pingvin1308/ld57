using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class InteractionArea : MonoBehaviour
    {
        public UnityEvent PlayerEntered;
        public UnityEvent PlayerExited;


        [field: SerializeField]
        public Player Player { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                Player = player;
                PlayerEntered?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                PlayerExited?.Invoke();
                Player = null;
            }
        }
    }
}