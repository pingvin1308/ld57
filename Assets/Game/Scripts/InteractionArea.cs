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

        public void Enter()
        {
            PlayerEntered?.Invoke();
        }

        public void Exit()
        {
            PlayerExited?.Invoke();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                Player = player;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                Player = null;
            }
        }

        private void OnDestroy()
        {
            PlayerEntered?.RemoveAllListeners();
            PlayerExited?.RemoveAllListeners();
        }
    }
}