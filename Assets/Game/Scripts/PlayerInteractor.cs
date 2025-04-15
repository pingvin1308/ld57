using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerInteractor : MonoBehaviour
    {
        private readonly List<InteractionArea> _areas = new();

        [field: SerializeField]
        public InteractionArea CurrentArea { get; private set; }

        private void Update()
        {
            var nearestArea = _areas
                .Where(x => x != null)
                .OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .FirstOrDefault();

            if (nearestArea != CurrentArea)
            {
                CurrentArea?.Exit();
                CurrentArea = nearestArea;
                CurrentArea?.Enter();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<InteractionArea>(out var area))
            {
                _areas.Add(area);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<InteractionArea>(out var area))
            {
                _areas.Remove(area);
            }
        }
    }
}