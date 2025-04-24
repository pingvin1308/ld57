using UnityEngine;

namespace Game.Scripts
{
    public class Game : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
        }
    }
}