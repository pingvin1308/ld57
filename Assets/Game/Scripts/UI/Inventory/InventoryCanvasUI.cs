using System;
using UnityEngine;

namespace Game.Scripts.UI.Inventory
{
    [RequireComponent(typeof(Canvas))]
    public class InventoryCanvasUI : MonoBehaviour
    {
        [field: SerializeField]
        public Canvas Canvas { get; private set; }

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }
    }
}