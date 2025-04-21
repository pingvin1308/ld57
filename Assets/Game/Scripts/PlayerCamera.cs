using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class PlayerCamera : MonoBehaviour
    {
        private CinemachineCamera _cinemachineCamera;

        private void Awake()
        {
            _cinemachineCamera = GetComponent<CinemachineCamera>();
        }

        public void SetTarget(Transform target)
        {
            _cinemachineCamera.Target.TrackingTarget = target;
        }
    }
}