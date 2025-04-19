using System.Collections;
using Game.Scripts.Dialog;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts
{
    public class Scenarios : MonoBehaviour
    {
        [field: SerializeField]
        public DialogRunner DialogRunner { get; private set; }

    }
}